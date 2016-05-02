$(function () {
    $(document).on('click', '.delete-person', function () {
        var personIdForDelete = $(this).data('id');
        $('#person-id-for-delete').val(personIdForDelete);
        $('#delete-form').submit();
    });
});