export function parseDate(date) {
    if (date instanceof Date) return date;
    var pattern = /^(\d+)\/(\d+)\/(\d+)$/;
    var strDate = (date || "").toString().trim();
    var jsFormat = strDate.replace(pattern, '$3-$2-$1');
    if (jsFormat !== strDate) {
        var splitted = jsFormat.split("-");
        if (splitted[0].length === 4) {
            return new Date(splitted[0], parseInt(splitted[1]) - 1, splitted[2]);
        }
    }

    return date;
}

Date.prototype.toString = function () {
    var date: Date = this;
    return `${date.getDate()}/${date.getMonth() + 1}/${date.getFullYear()}`;
}
