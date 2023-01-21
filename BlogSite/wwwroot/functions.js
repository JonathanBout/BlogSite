function GetDeviceWidth() {
	return Math.max(document.documentElement.clientWidth || 0, window.innerWidth || 0);
}

var GLOBAL = {};
GLOBAL.DotNetReference = null;
GLOBAL.SetDotnetReference = function (pDotNetReference) {
    GLOBAL.DotNetReference = pDotNetReference;
};

window.onresize = () => {
    if (GLOBAL.DotNetReference != null) {
        GLOBAL.DotNetReference.invokeMethodAsync('Resize');
    }
}