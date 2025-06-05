window.ShowToastr = function (type, message) {
    if (typeof toastr === "undefined") {
        console.error("Toastr is not loaded.");
        return;
    }

    if (type === "success") {
        toastr.success(message);
    } else if (type === "error") {
        toastr.error(message);
    }
}