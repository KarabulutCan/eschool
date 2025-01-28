/* The code in this project is licensed under the Apache 2.0 License(https://www.apache.org/licenses/LICENSE-2.0).
 * reCAPTCHA is a Copyright of  Google Inc. All rights reserved. */

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
"use strict";
var __extends = (this && this.__extends) || (function () {
    var extendStatics = Object.setPrototypeOf ||
        ({ __proto__: [] } instanceof Array && function (d, b) { d.__proto__ = b; }) ||
        function (d, b) { for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p]; };
    return function (d, b) {
        extendStatics(d, b);
        function __() { this.constructor = d; }
        d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
    };
})();
var kendo;
(function (kendo) {
    var ui;
    (function (ui) {
        var recaptcha;
        (function (recaptcha) {
            recaptcha.__meta__ = {
                id: "recaptcha",
                name: "reCAPTCHA",
                category: "web",
                description: "reCAPTCHA protects you against spam and other types of automated abuse. Here, we explain how to add reCAPTCHA to your site or application.",
                depends: ["core"]
            };
            recaptcha.CHANGE = "change", recaptcha.RESET = "reset", recaptcha.NS = ".kendoRecaptcha";
            var Recaptcha = (function (_super) {
                __extends(Recaptcha, _super);
                function Recaptcha(element, options) {
                    var _this = _super.call(this, element, $.extend(true, new recaptcha.RecaptchaOptions(), options)) || this;
                    _this.value = function (val) {
                        if (_this.isSet(val)) {
                            _this.element.val(val);
                        }
                        else {
                            return _this.element.val();
                        }
                    };
                    _this.getOptions = _this.getOptions.bind(_this);
                    _this.setOptions = _this.setOptions.bind(_this);
                    _this.clearOptions = _this.clearOptions.bind(_this);
                    _this.convertOptions = _this.convertOptions.bind(_this);
                    _this.onRecaptchaCallback = _this.onRecaptchaCallback.bind(_this);
                    _this.onRecaptchaExpiredCallback = _this.onRecaptchaExpiredCallback.bind(_this);
                    _this.renderRecaptcha = _this.renderRecaptcha.bind(_this);
                    _this.unrenderRecaptcha = _this.unrenderRecaptcha.bind(_this);
                    _this.renderRecaptchaWithCheck = _this.renderRecaptchaWithCheck.bind(_this);
                    _this.refresh = _this.refresh.bind(_this);
                    _this.destroy = _this.destroy.bind(_this);
                    _this._initEventHandler = _this._initEventHandler.bind(_this);
                    _this._change = _this._change.bind(_this);
                    _this.renderWrapper = _this.renderWrapper.bind(_this);
                    $(element).data("kendoRecaptcha", _this);
                    _this.events.push(recaptcha.CHANGE);
                    _this._initEventHandler();
                    _this.renderWrapper();
                    _this.renderRecaptcha();
                    return _this;
                }
                Recaptcha.prototype._initEventHandler = function () {
                    var _this = this;
                    this._changeHandler = $.proxy(this._change, this);
                    this.element.on(recaptcha.CHANGE, function (e) { return _this._changeHandler(e); });
                    var element = this.element;
                    var formId = element.attr("form");
                    var form = formId ? $("#" + formId) : element.closest("form");
                    if (form[0]) {
                        this._resetHandler = function (e) {
                            setTimeout(function () {
                                grecaptcha.reset(_this.recaptchaId);
                            });
                        };
                        this._form = form.on(recaptcha.RESET, function (e) { return _this._resetHandler(e); });
                    }
                };
                Recaptcha.prototype._change = function () {
                    this._value = this.element.val();
                    this.trigger(recaptcha.CHANGE, { value: this._value });
                };
                Recaptcha.prototype.destroy = function () {
                    ui.Widget.fn.destroy.call(this);
                    this.element.off(recaptcha.CHANGE);
                    this.element.off(recaptcha.NS);
                    this.element.empty();
                    this.recaptchaId = undefined;
                    kendo.destroy(this.element);
                };
                Recaptcha.prototype.getOptions = function () {
                    return this.options;
                };
                Recaptcha.prototype.clearOptions = function () {
                    this.options = new recaptcha.RecaptchaOptions();
                };
                Recaptcha.prototype.setOptions = function (options) {
                    var newoptions = $.extend(true, new recaptcha.RecaptchaOptions(), options);
                    _super.prototype.setOptions.call(this, newoptions);
                    this.refresh();
                };
                Recaptcha.prototype.convertOptions = function (options) {
                    var result = {
                        sitekey: options.sitekey,
                        theme: options.theme,
                        type: options.type,
                        size: options.size,
                        tabindex: options.tabindex,
                        badge: options.badge,
                        callback: this.onRecaptchaCallback,
                        "expired-callback": this.onRecaptchaExpiredCallback
                    };
                    return result;
                };
                Recaptcha.prototype.onRecaptchaExpiredCallback = function () {
                    this.element.val(undefined);
                    this._change();
                };
                Recaptcha.prototype.onRecaptchaCallback = function (response) {
                    this.element.val(response);
                    this._change();
                };
                Recaptcha.prototype.renderWrapper = function () {
                    var element = this.element;
                    var wrapper = element.parents(".k-recaptcha");
                    if (!wrapper.is("div.k-recaptcha")) {
                        wrapper = element.hide().wrap("<div/>").parent();
                    }
                    wrapper[0].style.cssText = element[0].style.cssText;
                    element[0].style.width = "";
                    this.wrapper = wrapper
                        .addClass("k-recaptcha")
                        .addClass(element[0].className)
                        .css("display", "");
                };
                Recaptcha.prototype.unrenderRecaptcha = function () {
                    if (this.isSet(this.recaptchaId)) {
                        grecaptcha.reset(this.recaptchaId);
                        this.recaptchaId = undefined;
                    }
                    if (this.isSet(this.recaptchaElement)) {
                        this.recaptchaElement.remove();
                        this.recaptchaElement = undefined;
                    }
                };
                Recaptcha.prototype.renderRecaptcha = function () {
                    if (this.isSet(this.recaptchaId)) {
                        this.unrenderRecaptcha();
                    }
                    this.renderRecaptchaWithCheck(0);
                };
                Recaptcha.prototype.renderRecaptchaWithCheck = function (counter) {
                    var _this = this;
                    var o = this.options;
                    if (counter > 20) {
                        return;
                    }
                    if (grecaptcha.render !== undefined) {
                        this.recaptchaElement = $("<div />").insertBefore(this.element);
                        this.recaptchaId = grecaptcha.render(this.recaptchaElement[0], this.convertOptions(o));
                    }
                    else {
                        setTimeout(function () { return _this.renderRecaptchaWithCheck(++counter); }, 150);
                    }
                };
                Recaptcha.prototype.refresh = function () {
                    this.renderRecaptcha();
                };
                Recaptcha.prototype.notSet = function (value) {
                    return value === null || value === undefined;
                };
                Recaptcha.prototype.isSet = function (value) {
                    return value !== null && value !== undefined;
                };
                return Recaptcha;
            }(kendo.ui.Widget));
            recaptcha.Recaptcha = Recaptcha;
            Recaptcha.fn = Recaptcha.prototype;
            Recaptcha.fn.options = new recaptcha.RecaptchaOptions();
            kendo.ui.plugin(Recaptcha);
        })(recaptcha = ui.recaptcha || (ui.recaptcha = {}));
    })(ui = kendo.ui || (kendo.ui = {}));
})(kendo || (kendo = {}));
//# sourceMappingURL=kendo.recaptcha.js.map