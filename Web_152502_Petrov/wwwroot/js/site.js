// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
$(document).ready(function () {
    $('a.page-link').click(function (event) {
        event.preventDefault();

        let url = $(this).attr('href');

        $.ajax({
            url: url,
            method: 'GET',
            success: function (response) {
                $('#picture-list').html(response);
                console.log('Successful AJAX request.')
            },
            error: function (xhr, status, error) {
                console.log('AJAX request failed: ${status}; ${error}');
            }
        });
    });
});