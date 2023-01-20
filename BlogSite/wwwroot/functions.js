function GetDeviceWidth() {
	return Math.max(document.documentElement.clientWidth || 0, window.innerWidth || 0);
}

window.onresize = () => {
	DotNet.invokeMethodAsync('BlogSite', 'Resize');
}