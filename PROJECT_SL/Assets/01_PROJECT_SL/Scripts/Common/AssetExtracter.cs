using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

namespace ProjectSL
{
    public class AssetExtracter
    {
        [MenuItem("ProjectSL/Export Used Assets")]
        public static void ExportUsedAssets()
        {
            // 현재 씬의 모든 루트 오브젝트 수집
            GameObject[] rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            // 루트 오브젝트를 기준으로 모든 참조된 Unity Object 추적
            Object[] dependencies = EditorUtility.CollectDependencies(rootObjects);

            // 저장할 경로
            string exportPath = "Assets/LargeAssets/UsedAssetPathsAtLargeAssets.txt";

            // 중복 경로 저장을 방지하기 위한 HashSet
            HashSet<string> uniqueAssetPaths = new HashSet<string>();

            foreach (Object obj in dependencies)
            {
                string path = AssetDatabase.GetAssetPath(obj);

                // 유효한 에셋 경로 (Assets/ 폴더 내에 있는 에셋만) 필터링
                if (!string.IsNullOrEmpty(path) && path.StartsWith("Assets/") && path.StartsWith("Assets/LargeAssets/"))
                {
                    uniqueAssetPaths.Add(path);
                }
            }

            // HashSet의 내용을 List로 변환하고 알파벳 순서대로 정렬
            List<string> sortedAssetPaths = uniqueAssetPaths.OrderBy(p => p).ToList();

            try
            {
                using (StreamWriter writer = new StreamWriter(exportPath))
                {
                    foreach (string path in sortedAssetPaths)
                    {
                        writer.WriteLine(path);
                    }
                }
                Debug.Log("Used asset paths saved to: " + exportPath);
                AssetDatabase.Refresh(); // 새로 생성된 파일을 유니티 에디터에 반영
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to save asset paths to {exportPath}: {e.Message}");
            }
        }
    }
}
