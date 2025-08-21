mergeInto(LibraryManager.library, {
    SaveToLocalStorage: function (key, value) {
        localStorage.setItem(UTF8ToString(key), UTF8ToString(value));
    },
    LoadFromLocalStorage: function (key) {
        var value = localStorage.getItem(UTF8ToString(key));
        return value ? allocateUTF8(value) : 0;
    }
});