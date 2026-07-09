window.app = {
    async toggleFullscreen() {
        if (!document.fullscreenElement) {
            await document.documentElement.requestFullscreen();
            return true;
        }

        await document.exitFullscreen();
        return false;
    },

    downloadFileFromText(fileName, fileContent) {
        const blob = new Blob([fileContent], { type: "text/csv;charset=utf-8" });
        const url = URL.createObjectURL(blob);
        const anchor = document.createElement("a");
        anchor.href = url;
        anchor.download = fileName;
        document.body.appendChild(anchor);
        anchor.click();
        anchor.remove();
        URL.revokeObjectURL(url);
    }
};
