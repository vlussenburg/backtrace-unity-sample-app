using Backtrace.Unity;
using Backtrace.Unity.Model;
using Backtrace.Unity.Model.Breadcrumbs;
using System.Collections.Generic;
using UnityEngine;

public class BacktraceManager : MonoBehaviour
{
    BacktraceClient _client;
    private const string BacktraceGameObjectName = "Backtrace";
    private readonly Dictionary<string, string> _attributes = new Dictionary<string, string>()
        {
            {"author", "Konrad" },
            { "project-name", "backtrace-unity validation" },
            { "pid", System.Diagnostics.Process.GetCurrentProcess().Id.ToString() },
            { "test-nullable-variable", null },
            { "test-empty-variable", null },
            { "long-string", GetReallyLongString() },
        };
    void Start()
    {
        var attributes =
        _client = GameObject.Find(BacktraceGameObjectName)?.GetComponent<BacktraceClient>();
        if (_client == null)
        {
            Debug.Log("Initializing Backtrace client via API interface");
            var configuration = ScriptableObject.CreateInstance<BacktraceConfiguration>();
            configuration.ServerUrl = "backtrace-server-url";
            configuration.Enabled = true;
            configuration.PerformanceStatistics = true;
            configuration.EnableMetricsSupport = true;
            var databasePath = "${Application.persistentDataPath}/backtrace";
            configuration.DatabasePath = databasePath;
            configuration.CreateDatabase = true;
            configuration.EnableBreadcrumbsSupport = true;
            configuration.BacktraceBreadcrumbsLevel = BacktraceBreadcrumbType.Configuration | BacktraceBreadcrumbType.Http | BacktraceBreadcrumbType.Log | BacktraceBreadcrumbType.Manual | BacktraceBreadcrumbType.System | BacktraceBreadcrumbType.Navigation;
            configuration.LogLevel = UnityEngineLogLevel.Debug | UnityEngineLogLevel.Error | UnityEngineLogLevel.Fatal | UnityEngineLogLevel.Info | UnityEngineLogLevel.Warning;
#if UNITY_ANDROID || UNITY_IOS
            configuration.OomReports = true;
            configuration.HandleANR = false;
            configuration.CaptureNativeCrashes = true;
            //   configuration.SymbolsUploadToken = "f3228a2c0ec3167f9235a0bd039d4ff4ccf9a21b227a5f1156808305f3e7b3b6";
            //configuration.ClientSideUnwinding = true;
#endif
            _client = BacktraceClient.Initialize(configuration, _attributes, BacktraceGameObjectName);
            _client.EnableMetrics();
            Debug.Log("Successfully started Backtrace client integration");
        }
        else
        {
            Debug.Log("Backtrace interface initialized via GameObject integration");
        }
        _client.SetAttributes(_attributes);
    }

    private static string GetReallyLongString()
    {
        return @"Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ac odio tempor orci dapibus ultrices in iaculis nunc sed. Leo a diam sollicitudin tempor. Velit egestas dui id ornare arcu. Sed faucibus turpis in eu mi bibendum neque egestas. Nunc sed id semper risus in. Semper auctor neque vitae tempus quam pellentesque. Ac auctor augue mauris augue neque. Rutrum tellus pellentesque eu tincidunt tortor aliquam. Commodo odio aenean sed adipiscing.

Quam nulla porttitor massa id neque. Magna sit amet purus gravida quis blandit turpis cursus in. Eget duis at tellus at urna condimentum mattis pellentesque id. Nisl rhoncus mattis rhoncus urna neque viverra. Viverra aliquet eget sit amet tellus cras adipiscing enim eu. Diam vulputate ut pharetra sit amet aliquam id diam maecenas. Fringilla urna porttitor rhoncus dolor purus non enim. Feugiat vivamus at augue eget. Arcu non odio euismod lacinia at quis risus. Porttitor lacus luctus accumsan tortor posuere. Diam sit amet nisl suscipit adipiscing bibendum. Cursus mattis molestie a iaculis at erat. Feugiat scelerisque varius morbi enim. Vulputate eu scelerisque felis imperdiet proin fermentum leo vel. Nisi lacus sed viverra tellus in hac habitasse platea. Elit duis tristique sollicitudin nibh. Eleifend mi in nulla posuere sollicitudin.

Hendrerit gravida rutrum quisque non tellus. Eget velit aliquet sagittis id consectetur purus ut faucibus pulvinar. Malesuada pellentesque elit eget gravida cum sociis. Integer malesuada nunc vel risus commodo viverra. Nulla aliquet porttitor lacus luctus accumsan tortor. Magnis dis parturient montes nascetur ridiculus. Facilisi etiam dignissim diam quis. Pellentesque habitant morbi tristique senectus et netus et malesuada. Non quam lacus suspendisse faucibus interdum posuere lorem ipsum. Convallis a cras semper auctor neque vitae tempus quam pellentesque. Ipsum dolor sit amet consectetur. At tellus at urna condimentum. Pretium lectus quam id leo in vitae turpis. Ut pharetra sit amet aliquam id. Ac turpis egestas maecenas pharetra convallis posuere morbi leo. Varius quam quisque id diam.

Vitae congue eu consequat ac felis donec et odio. Pulvinar elementum integer enim neque volutpat ac tincidunt. Lorem dolor sed viverra ipsum nunc aliquet bibendum enim facilisis. Arcu risus quis varius quam quisque id diam. Eleifend mi in nulla posuere sollicitudin aliquam. Tempor id eu nisl nunc. Commodo nulla facilisi nullam vehicula. Aliquam nulla facilisi cras fermentum odio. Pellentesque id nibh tortor id. Pharetra magna ac placerat vestibulum lectus mauris ultrices eros in. Sed egestas egestas fringilla phasellus faucibus scelerisque. Amet porttitor eget dolor morbi. Quisque sagittis purus sit amet volutpat consequat. Lectus magna fringilla urna porttitor rhoncus dolor. Auctor elit sed vulputate mi sit amet. Etiam tempor orci eu lobortis elementum nibh tellus molestie nunc. Risus at ultrices mi tempus imperdiet nulla malesuada.

Odio ut sem nulla pharetra diam sit amet nisl suscipit. Sed enim ut sem viverra aliquet eget. Eget nullam non nisi est sit. Senectus et netus et malesuada fames ac turpis. Eu augue ut lectus arcu bibendum at. Neque gravida in fermentum et sollicitudin ac orci phasellus egestas. Convallis posuere morbi leo urna molestie at elementum eu. Praesent semper feugiat nibh sed. Faucibus purus in massa tempor nec feugiat nisl pretium fusce. Egestas fringilla phasellus faucibus scelerisque eleifend donec pretium. Dui faucibus in ornare quam viverra orci sagittis eu volutpat. Arcu dictum varius duis at consectetur lorem donec massa. Vitae tempus quam pellentesque nec nam aliquam. Laoreet suspendisse interdum consectetur libero. Vel orci porta non pulvinar neque laoreet suspendisse. Nunc non blandit massa enim nec dui nunc. Dolor sit amet consectetur adipiscing elit. Nibh sit amet commodo nulla facilisi nullam vehicula.

Egestas sed tempus urna et pharetra pharetra massa. Dui id ornare arcu odio ut sem. Justo nec ultrices dui sapien. Libero volutpat sed cras ornare arcu. Faucibus interdum posuere lorem ipsum dolor sit amet consectetur adipiscing. Tincidunt eget nullam non nisi est sit amet facilisis magna. Faucibus vitae aliquet nec ullamcorper sit amet. Ut porttitor leo a diam sollicitudin. Risus sed vulputate odio ut enim. Donec ultrices tincidunt arcu non sodales neque sodales ut. Ut morbi tincidunt augue interdum velit euismod in pellentesque. Est velit egestas dui id ornare arcu. Diam vel quam elementum pulvinar etiam non quam lacus suspendisse.

Quam quisque id diam vel quam elementum pulvinar. Suspendisse interdum consectetur libero id faucibus nisl tincidunt. Ut consequat semper viverra nam libero justo laoreet. Diam vulputate ut pharetra sit amet. Ultrices sagittis orci a scelerisque purus semper eget duis at. Tellus pellentesque eu tincidunt tortor. Porta lorem mollis aliquam ut. Eleifend mi in nulla posuere sollicitudin aliquam. Tristique et egestas quis ipsum suspendisse ultrices gravida. Penatibus et magnis dis parturient montes nascetur ridiculus mus mauris. Sed lectus vestibulum mattis ullamcorper velit sed ullamcorper. Pharetra massa massa ultricies mi quis hendrerit dolor. Molestie ac feugiat sed lectus vestibulum. Eu non diam phasellus vestibulum lorem sed risus ultricies tristique. Non tellus orci ac auctor augue mauris. Habitasse platea dictumst vestibulum rhoncus est pellentesque elit. Aliquet nibh praesent tristique magna sit amet.

Elit eget gravida cum sociis natoque penatibus et magnis dis. Etiam tempor orci eu lobortis. Etiam non quam lacus suspendisse. Et pharetra pharetra massa massa ultricies mi quis hendrerit. Egestas quis ipsum suspendisse ultrices gravida. Suscipit adipiscing bibendum est ultricies integer. Morbi tempus iaculis urna id volutpat lacus laoreet non curabitur. Sed tempus urna et pharetra pharetra massa. Rutrum quisque non tellus orci. Iaculis at erat pellentesque adipiscing commodo elit at.

Porta lorem mollis aliquam ut porttitor. Sit amet nulla facilisi morbi. Urna id volutpat lacus laoreet non curabitur gravida. Sit amet cursus sit amet dictum sit amet. Amet volutpat consequat mauris nunc congue. Vestibulum lorem sed risus ultricies tristique nulla aliquet. Suscipit tellus mauris a diam maecenas sed enim ut sem. Ornare arcu odio ut sem nulla pharetra diam sit. Venenatis cras sed felis eget velit aliquet. Arcu cursus euismod quis viverra. Aliquam id diam maecenas ultricies mi eget mauris pharetra et. Urna condimentum mattis pellentesque id nibh tortor id aliquet lectus. Lacus sed viverra tellus in hac habitasse. Vivamus at augue eget arcu dictum varius duis at.

Morbi tristique senectus et netus. Morbi quis commodo odio aenean sed adipiscing. Pulvinar proin gravida hendrerit lectus a. Nunc mattis enim ut tellus elementum sagittis. Neque gravida in fermentum et sollicitudin ac orci. Duis tristique sollicitudin nibh sit amet commodo nulla. Tempus egestas sed sed risus pretium. At volutpat diam ut venenatis tellus in. Risus sed vulputate odio ut enim blandit volutpat maecenas. Ut porttitor leo a diam sollicitudin tempor. Diam vel quam elementum pulvinar etiam non. In vitae turpis massa sed elementum tempus egestas sed sed. In egestas erat imperdiet sed euismod. Mauris augue neque gravida in. Faucibus turpis in eu mi. Maecenas accumsan lacus vel facilisis volutpat. Nec sagittis aliquam malesuada bibendum. Condimentum id venenatis a condimentum. Sem integer vitae justo eget magna fermentum. Dui accumsan sit amet nulla.

Justo nec ultrices dui sapien eget mi proin. Tempor nec feugiat nisl pretium fusce id velit ut. Sit amet aliquam id diam maecenas ultricies mi eget. Neque ornare aenean euismod elementum nisi quis eleifend. Habitant morbi tristique senectus et. Mauris rhoncus aenean vel elit scelerisque. Et pharetra pharetra massa massa ultricies mi quis hendrerit dolor. Lacinia quis vel eros donec ac odio tempor orci dapibus. Egestas maecenas pharetra convallis posuere morbi leo urna molestie. Aliquam faucibus purus in massa. In nisl nisi scelerisque eu. Vitae congue mauris rhoncus aenean. Ut venenatis tellus in metus vulputate eu. Nunc mi ipsum faucibus vitae aliquet nec ullamcorper sit. Elementum sagittis vitae et leo. Viverra ipsum nunc aliquet bibendum enim facilisis gravida neque convallis.

Nibh praesent tristique magna sit amet purus. Vivamus at augue eget arcu dictum. Luctus venenatis lectus magna fringilla. Eleifend mi in nulla posuere sollicitudin aliquam. Blandit cursus risus at ultrices. Leo a diam sollicitudin tempor id eu. Tincidunt praesent semper feugiat nibh. Ipsum dolor sit amet consectetur. Vel facilisis volutpat est velit egestas dui id ornare. Pharetra vel turpis nunc eget lorem dolor sed viverra. Imperdiet proin fermentum leo vel. Hac habitasse platea dictumst quisque. In hac habitasse platea dictumst. Lectus urna duis convallis convallis tellus id. Fermentum iaculis eu non diam. Ornare arcu dui vivamus arcu felis. Velit dignissim sodales ut eu.

Nec nam aliquam sem et tortor consequat id porta. In iaculis nunc sed augue. Sit amet tellus cras adipiscing enim eu turpis egestas pretium. Elit eget gravida cum sociis natoque penatibus. Sapien eget mi proin sed. Lacus vel facilisis volutpat est velit egestas. Porttitor leo a diam sollicitudin tempor id eu nisl. Scelerisque eleifend donec pretium vulputate sapien nec sagittis aliquam. Fermentum posuere urna nec tincidunt. Eu mi bibendum neque egestas congue quisque egestas diam. Sed odio morbi quis commodo. Purus gravida quis blandit turpis cursus in hac. Est velit egestas dui id. Nullam non nisi est sit amet facilisis. Orci phasellus egestas tellus rutrum tellus. Ut diam quam nulla porttitor massa id neque aliquam vestibulum. Nulla aliquet enim tortor at auctor. Arcu dui vivamus arcu felis bibendum ut tristique et. Sed adipiscing diam donec adipiscing tristique risus nec. Volutpat blandit aliquam etiam erat velit scelerisque in dictum.

Ultrices neque ornare aenean euismod elementum. In tellus integer feugiat scelerisque varius morbi enim nunc faucibus. Euismod nisi porta lorem mollis aliquam ut porttitor leo a. Eleifend quam adipiscing vitae proin sagittis nisl rhoncus mattis. Dolor morbi non arcu risus quis varius quam quisque. Tincidunt lobortis feugiat vivamus at augue eget arcu. Risus feugiat in ante metus dictum at tempor. Ridiculus mus mauris vitae ultricies leo. Pellentesque elit eget gravida cum sociis. Aliquet enim tortor at auctor urna. Elementum pulvinar etiam non quam lacus suspendisse faucibus interdum. Feugiat nibh sed pulvinar proin gravida hendrerit. Faucibus purus in massa tempor nec. Pulvinar mattis nunc sed blandit libero volutpat sed cras. Tortor id aliquet lectus proin nibh nisl.

Amet nisl purus in mollis nunc sed id. Tellus integer feugiat scelerisque varius morbi. Ultrices tincidunt arcu non sodales neque sodales ut etiam sit. In nulla posuere sollicitudin aliquam ultrices sagittis. Proin fermentum leo vel orci porta non. Sed augue lacus viverra vitae congue eu consequat ac. Porttitor massa id neque aliquam vestibulum morbi blandit. Amet venenatis urna cursus eget nunc scelerisque viverra. Laoreet sit amet cursus sit amet dictum sit amet. Est sit amet facilisis magna etiam tempor orci eu. Enim nulla aliquet porttitor lacus luctus.";
    }
}
