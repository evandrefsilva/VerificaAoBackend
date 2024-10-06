// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


function search() {
    const province = document.getElementById('provincia').value

    const modalBootstrap = new bootstrap.Modal(document.getElementById('loading'), {
        keyboard: false,
    });
    modalBootstrap.show();
}

function returnClassStatus(status) {
    if (status === 'Aceite') return 'bg-gradient-success'
    if (status == 'Rejeitado') return 'bg-gradient-danger'
    return 'bg-gradient-secondary'
}