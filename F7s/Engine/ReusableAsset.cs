using F7s.Mains;
using Stride.Core.Serialization.Contents;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace F7s.Engine;

public class ReusableAssets {

    private static ContentManager contentManager;
    private static readonly Dictionary<string, object> preloaded = new Dictionary<string, object>();


    public static T Reuse<T> (string url, Func<T> builder = null) where T : class {

        if (preloaded.ContainsKey(url)) {
            return (T) preloaded[url];
        }

        if (contentManager == null) {
            contentManager = MainSync.ContentManager;
        }

        if (contentManager.Exists(url)) {
            Debug.WriteLine("Loading " + url + ".");
            object raw = contentManager.Load(typeof(T), url);
            T asset = (T) raw;
            preloaded.Add(url, asset);
            return asset;

        }

        if (builder != null) {
            T asset = builder.Invoke();
            preloaded.Add(url, asset);
            contentManager.Save(url, asset);
            Debug.WriteLine("Saving " + url + ".");
            return asset;
        }

        return null;
    }

}
