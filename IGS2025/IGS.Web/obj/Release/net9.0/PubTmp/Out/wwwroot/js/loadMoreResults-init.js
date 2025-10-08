$(document).ready(function(){
    // LoadMore Plugin Configuration
    $('.loadMore').loadMoreResults({
        displayedItems: 12,
        showItems: 4,
        button: {
            'class': 'btn-load-more',
            'text': 'Load More'
          }
    })
})