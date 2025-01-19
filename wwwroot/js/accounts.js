$(".btn_toggleAddAccount").on("click", function () {
    if ($("#addAccountContainer").is(":hidden"))
    {
        $("#btn_toggleAddAccount").fadeToggle().promise().done(function () {
            $("#addAccountContainer").fadeToggle();
        });
    }
    else
    {
        $("#addAccountContainer").fadeToggle().promise().done(function () {
            $("#btn_toggleAddAccount").fadeToggle();
            $("#field_addAccount").val("");
        });
    }
});

$(".btn_toggleEditAccount").on("click", function () {


    $(this).parent().parent().find("#txt_accountName").toggle();
    $(this).parent().parent().find("#field_editAccount").val($(this).parent().parent().find("#txt_accountName").text());
    $(this).parent().parent().find("#editAccountContainer").toggle();
    $(this).parent().parent().find("#container_options").toggle();
});

$(".btn_abortEditAccount").on("click", function () {


    $(this).parent().parent().parent().find("#txt_accountName").toggle();
    $(this).parent().parent().parent().find("#field_editAccount").val($(this).parent().parent().find("#txt_accountName").text());
    $(this).parent().parent().parent().find("#editAccountContainer").toggle();
    $(this).parent().parent().parent().find("#container_options").toggle();
});

$(".btn_toggledelete").on("click", function () {

    $(this).parent().parent().find("#txt_accountName").toggle();
    $(this).parent().parent().find("#container_options").toggle();
    $(this).parent().parent().find("#delete_sure").toggle();
});