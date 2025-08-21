mergeInto(LibraryManager.library, {
    DownloadFile: function (jsonPtr, filenamePtr) {
        var json = UTF8ToString(jsonPtr);
        var filename = UTF8ToString(filenamePtr);

        var blob = new Blob([json], { type: "application/json" });
        var link = document.createElement("a");
        link.href = URL.createObjectURL(blob);
        link.download = filename;
        link.click();
        URL.revokeObjectURL(link.href);
    }
});
