"use strict";
var kendo;
(function (kendo) {
    var ui;
    (function (ui) {
        var recaptcha;
        (function (recaptcha) {
            var RecaptchaOptions = (function () {
                function RecaptchaOptions() {
                    this.name = "recaptcha";
                    this.autoBind = true;
                    this.sitekey = "";
                    this.secretkey = "";
                    this.theme = "light";
                    this.type = "image";
                    this.size = "compact";
                    this.badge = "bottomright";
                }
                return RecaptchaOptions;
            }());
            recaptcha.RecaptchaOptions = RecaptchaOptions;
        })(recaptcha = ui.recaptcha || (ui.recaptcha = {}));
    })(ui = kendo.ui || (kendo.ui = {}));
})(kendo || (kendo = {}));
//# sourceMappingURL=kendo.recaptcha.options.js.map