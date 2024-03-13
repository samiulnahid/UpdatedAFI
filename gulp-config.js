module.exports = function () {
  var instanceRoot = "";
  var config = {
     //Overridden
    websiteRoot: instanceRoot + "\\Website",
    sitecoreLibraries: instanceRoot + "\\Website\\bin",
    buildToolsVersion: 15.0,
    buildMaxCpuCount: 0,
    buildVerbosity: "detailed",
    buildPlatform: "Any CPU",
    publishPlatform: "AnyCpu",
    runCleanBuilds: false
  };
  return config;
}
