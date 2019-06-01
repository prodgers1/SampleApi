$('body').on('keyup', '.search', function () {
    let cards = $('.card');
    let search = $(this).val();
    $.each(cards, function () {
        let text = $(this).text().trim();
        
        if (text.indexOf(search) !== -1)
            $(this).show();

        else
            $(this).hide();
    })
})