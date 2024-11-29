function toggleForm(formType) {
    const loginForm = document.getElementById('loginForm');
    const registerForm = document.getElementById('registerForm');
    if (formType === 'login') {
        loginForm.classList.remove('hidden');
        registerForm.classList.add('hidden');
    } else {
        loginForm.classList.add('hidden');
        registerForm.classList.remove('hidden');
    }
}
