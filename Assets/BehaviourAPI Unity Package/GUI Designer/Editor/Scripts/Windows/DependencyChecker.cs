#if UNITY_EDITOR

using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public  class DependencyChecker
{
    string RequiredPackage;
    ListRequest request;
   public static bool isInstalled;

    public DependencyChecker(string package)
    {
        RequiredPackage = package;
        request = Client.List(true);
        EditorApplication.update += Poll;
    }

    void Poll()
    {
        if (!request.IsCompleted) { isInstalled = true; return; }
        EditorApplication.update -= Poll;

        if (request.Status == StatusCode.Success)
        {
            bool found = false;
            foreach (var pkg in request.Result)
                if (pkg.name == RequiredPackage) { found = true; break; }

            if (!found)
            {
                if (EditorUtility.DisplayDialog("Paquete Necesario",
                    $"La API requiere el paquete {RequiredPackage} para el funcionamiento de Copiado/Pegado de Nodos.",
                    "Instalar", "Continuar sin Copy/Paste"))
                {
                    Client.Add(RequiredPackage);
                    isInstalled = true;
                }
                else
                {
                    isInstalled = false;
                }
            }
        }
        else Debug.LogError(request.Error.message);
    }
}

#endif