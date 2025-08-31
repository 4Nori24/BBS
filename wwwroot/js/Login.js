document.addEventListener("DOMContentLoaded", function () {
    const btnLogin = document.getElementById("btnLogin");
    const btnRegister = document.getElementById("btnRegister");
    const passwordConfirmDiv = document.getElementById("passwordConfirm");

    let isRegisterMode = false;

    // 初期状態では確認欄を非表示にしておく
    passwordConfirmDiv.style.display = "none";

    btnRegister.addEventListener("click", function () {
        isRegisterMode = !isRegisterMode;

        if (isRegisterMode) {
            // 登録モードに切り替え
            btnLogin.textContent = "登録";
            btnLogin.value = "register";
            btnRegister.textContent = "戻る";
            passwordConfirmDiv.style.display = "block";
        } else {
            // ログインモードに戻す
            btnLogin.textContent = "ログイン";
            btnLogin.value = "login";
            btnRegister.textContent = "新規登録";
            passwordConfirmDiv.style.display = "none";
        }
    });
});
