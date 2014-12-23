/*
 * 
 * (c) Copyright Ascensio System SIA 2010-2014
 * 
 * This program is a free software product.
 * You can redistribute it and/or modify it under the terms of the GNU Affero General Public License
 * (AGPL) version 3 as published by the Free Software Foundation. 
 * In accordance with Section 7(a) of the GNU AGPL its Section 15 shall be amended to the effect 
 * that Ascensio System SIA expressly excludes the warranty of non-infringement of any third-party rights.
 * 
 * This program is distributed WITHOUT ANY WARRANTY; 
 * without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. 
 * For details, see the GNU AGPL at: http://www.gnu.org/licenses/agpl-3.0.html
 * 
 * You can contact Ascensio System SIA at Lubanas st. 125a-25, Riga, Latvia, EU, LV-1021.
 * 
 * The interactive user interfaces in modified source and object code versions of the Program 
 * must display Appropriate Legal Notices, as required under Section 5 of the GNU AGPL version 3.
 * 
 * Pursuant to Section 7(b) of the License you must retain the original Product logo when distributing the program. 
 * Pursuant to Section 7(e) we decline to grant you any rights under trademark law for use of our trademarks.
 * 
 * All the Product's GUI elements, including illustrations and icon sets, as well as technical 
 * writing content are licensed under the terms of the Creative Commons Attribution-ShareAlike 4.0 International. 
 * See the License terms at http://creativecommons.org/licenses/by-sa/4.0/legalcode
 * 
*/

/*
Copyright (c) Ascensio System SIA 2013. All rights reserved.
http://www.teamlab.com
*/
window.TariffPartner = (function () {
    var isInit = false;

    var init = function () {
        if (isInit) {
            return;
        }
        isInit = true;

        jq("#registrationKeyValue").on("keyup change paste", function (e) {
            setTimeout(changeCodeValue, 0);
            return e.type == "paste";
        });

        jq("#partnerPayKeyDialog").on("change", "input:radio", function () {
            var isKeyEnter = jq("#registrationKeyOption").prop("checked");
            selectOption(isKeyEnter);
        });

        jq("#partnerPayKeyDialog").on("click", ".tariff-key-aplly:not(.disable)", function () {
            activateKey();
            return false;
        });

        jq("#partnerPayKeyDialog").on("click", ".tariff-key-request:not(.disable)", function () {
            requestKey();
            return false;
        });

        jq(".tariffs-button-block").on("click", ".tariff-pay-pal:not(.disable)", function () {
            payPal();
            return false;
        });
        
        jq("#partnerPayKeyDialog").on("click", ".tariff-key-cancel:not(.disable)", function () {
            PopupKeyUpActionProvider.CloseDialog();
            return false;
        });

        jq(".tariffs-button-block").on("click", ".tariff-pay-key:not(.disable)", function () {
            TariffPartner.showPayKeyDialog();
            return false;
        });
    };

    var validKey = function (key) {
        return key && key.length > 0 && key.length < 256;
    };

    var showPayKeyDialog = function () {
        jq("#registrationKeyValue").val("");
        jq(".tariff-key-aplly, .tariff-key-request, .tariff-key-cancel").removeClass("disable");
        jq("#partnerPayKeyDialog input").prop("disabled", false);
        LoadingBanner.hideLoaderBtn("#partnerPayKeyDialog");
        jq("#partnerPayKeyDialog .error-popup").hide();
        changeCodeValue();
        TariffSettings.selectTariff();

        StudioBlockUIManager.blockUI("#partnerPayKeyDialog", 350, 300, 0);

        selectOption(true);
    };

    var selectOption = function (isKeyEnter) {
        isKeyEnter = isKeyEnter === true;
        jq("#registrationKeyOption").prop("checked", isKeyEnter);
        jq("#registrationRequestOption").prop("checked", !isKeyEnter);

        jq(".tariff-key-aplly").toggle(isKeyEnter);
        jq(".tariff-key-request").toggle(!isKeyEnter);
        
        if (isKeyEnter) {
            jq("#registrationKeyValue").focus();
        }
    };

    var changeCodeValue = function () {
        var key = jq("#registrationKeyValue").val().trim();

        jq(".tariff-key-aplly").toggleClass("disable", !validKey(key));
        selectOption(true);
    };

    var activateKey = function () {
        var key = jq("#registrationKeyValue").val().trim();
        if (!validKey(key)) {
            return;
        }

        jq(".tariff-key-aplly, .tariff-key-request, .tariff-key-cancel").addClass("disable");
        jq("#partnerPayKeyDialog input").prop("disabled", true);
        LoadingBanner.showLoaderBtn("#partnerPayKeyDialog");
        jq("#partnerPayKeyDialog .error-popup").hide();
        
        var timeout = AjaxPro.timeoutPeriod;
        AjaxPro.timeoutPeriod = 60 * 1000;
        TariffPartnerController.ActivateKey(key,
            function (result) {
                jq(".tariff-key-aplly, .tariff-key-request, .tariff-key-cancel").removeClass("disable");
                jq("#partnerPayKeyDialog input").prop("disabled", false);
                LoadingBanner.hideLoaderBtn("#partnerPayKeyDialog");
                jq("#partnerPayKeyDialog .error-popup").hide();
                if (result.error != null) {
                    jq("#partnerPayKeyDialog .error-popup").show().text(ASC.Resources.Master.Resource.ErrorKeyActivation);
                    return;
                }

                PopupKeyUpActionProvider.EnterAction = "PopupKeyUpActionProvider.CloseDialog();";
                PopupKeyUpActionProvider.CloseDialogAction = "location.reload();";
                StudioBlockUIManager.blockUI("#partnerApplyDialog", 350, 300, 0);
                setTimeout("location.reload();", 5000);
                AjaxPro.timeoutPeriod = timeout;
            });
    };

    var requestKey = function () {
        jq(".tariff-key-aplly, .tariff-key-request, .tariff-key-cancel").addClass("disable");
        jq("#partnerPayKeyDialog input").prop("disabled", true);
        LoadingBanner.showLoaderBtn("#partnerPayKeyDialog");
        jq("#partnerPayKeyDialog .error-popup").hide();
        
        var quotaId = TariffSettings.selectedQuotaId();
        TariffPartnerController.RequestKey(quotaId,
            function (result) {
                jq(".tariff-key-aplly, .tariff-key-request, .tariff-key-cancel").removeClass("disable");
                jq("#partnerPayKeyDialog input").prop("disabled", false);
                jq("#partnerPayKeyDialog .error-popup").hide();
                LoadingBanner.hideLoaderBtn("#partnerPayKeyDialog");
                if (result.error != null) {
                    jq("#partnerPayKeyDialog .error-popup").show().text(result.error.Message);
                    return;
                }

                PopupKeyUpActionProvider.EnterAction = "PopupKeyUpActionProvider.CloseDialog();";
                StudioBlockUIManager.blockUI("#partnerRequestDialog", 350, 300, 0);
            });
    };

    var payPal = function () {
        var quotaId = TariffSettings.selectedQuotaId();
        TariffPartnerController.RequestPayPal(quotaId,
            function (result) {
                var res = result.error || result.value;
                if (res.Message || res.message) {
                    jq("#partnerPayExceptionText").html(res.Message || res.message);
                    PopupKeyUpActionProvider.EnterAction = "PopupKeyUpActionProvider.CloseDialog();";
                    StudioBlockUIManager.blockUI("#partnerPayExceptionDialog", 350);
                    return;
                }

                if (result.value.rs2 == "quotaexceed") {
                    TariffSettings.showDowngradeDialog();
                    return;
                }

                location.href = result.value.rs1;
            });
    };

    return {
        init: init,

        showPayKeyDialog: showPayKeyDialog
    };
})();

jq(function () {
    TariffPartner.init();
});
