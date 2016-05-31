
$( window ).load(function() {
    imageCover();

})

$(document).ready(function() {
    /*Slider*/
    $('.slider-inner').carousel({
        auto: false,
        animateSpeed : 1000,
        autoSpeed    : 4000
    });

    //Arrow horizontal align
    sliderControler();

    //Mobile Navigation
    $('.navbar-toggle').click( function() {
        var navWidth = $('.nav-mobile').width();
        if( $('.main-inner').hasClass('slide-left') ) {
            $('.main-inner').removeClass('slide-left');
            $('.main-inner').animate( {
                left: 0
            }, 'slow')
        } else {
            $('.main-inner').addClass('slide-left');
            $('.main-inner').animate( {
                left: - navWidth
            }, 'slow')
        }
    })

     //Add placeholder as a value
    $('input[type=text]').each( function() {
        if( $(this).val() == '' && $(this).attr('placeholder') != undefined ) {
            $(this).val( $(this).attr('placeholder') );
        }
    });
    
    //Clear value on focus
    $('input').each(function() {
        placeholder(this);
    })

    //For Down arrow
    $('.down-arrow').click(function() {
        var headerHeight = $("header").outerHeight(true);
        var headerContentHeight = $("header + section").outerHeight(true);
        var sliderHeight = $(".carousel").outerHeight(true);
        var totalHeight = headerHeight + headerContentHeight + sliderHeight;
    
        $('main, .main-inner, html, body').animate( {
            scrollTop: totalHeight
        }, 1500);
        
        
    })
    //For Up arrow
    
    $('.back-top').click(function(e) {
        e.preventDefault();
        $('main, html, body').animate( {
            scrollTop: 0
        }, 1500);
    });
    
    

    /*$("#accordion a, .nav-tabs li a").click(function() {
        setTimeout(function() {
            autoHeight();
        }, 200);
        
    });*/

    getSliderHeight();
});

if($(window).width() <=1024) {
    autoHeight();    
} else {
    $('.page-height').removeAttr('style');
}


//For Resize window
$(window).resize(function() {
    if($('.slider').length) {
        imageCover();
    }
    $('.main-inner').removeClass('.slide-left').css( {'left': 0} )
    sliderControler();

    if($(window).width() <=1024) {
        autoHeight();    
    } else {
        $('.page-height').removeAttr('style');
    }
    
    getSliderHeight();
});

function sliderControler() {
    var windowWidth     = $(window).width(),
        contentWidth    = $('.slider-content').width(),
        spacingLeft     = ((windowWidth - contentWidth) / 4) - 27;

        $('.prev').css('left', spacingLeft);
        $('.next').css('right', spacingLeft);
}

function imageCover() {
    $('.slider .active img').css({"width":"auto", "height":"auto"});
    var windowWidth = $(window).width();
    var windowHeight = $(window).height();

    var imageWidth = $('.slider .active img').width();
    var imageHeight = $('.slider .active img').height();

    var widthRatio = windowWidth/imageWidth;
    var heightRatio = windowHeight/imageHeight;

    if(widthRatio > heightRatio) {
        $('.slider .active img').css({"width":"100%", "height":"auto"});
    }
    else {
        $('.slider .active img').css({"height":"100%", "width":"auto"});
    }

    carouselCenter();
}

function carouselCenter() {
    var carouselLeft = '-' + ($(".slider .active img").width() - $(window).width())/2;
    var carouselTop = '-' + ($(".slider .active img").height() - $(window).height())/2;

    $('.slider .active img').css("margin-left",0 + 'px');
    $('.slider .active img').css("margin-top",0 + 'px');

}

function autoHeight() {
    ////alert($("html").outerHeight(true));
    $(".page-height").removeAttr('style');
    setTimeout(function() {
        var windowHeight = $(window).outerHeight(true)- $("footer").outerHeight();
        var documentHeight = $(document).height() - $("footer").outerHeight();

        if(documentHeight > windowHeight) {
            $(".page-height").css("min-height", documentHeight);    
        } else {
            $(".page-height").css("min-height", windowHeight);
        }

    }, 200);
    
}


function getSliderHeight() {
    var windowHeight = $(window).outerHeight(true);
    var headerHeight = $("header").outerHeight(true);
    var headerContent = $("header + section").outerHeight(true);
    var navWidth = $(".slider-tabbing").outerHeight(true);
    var totalHeight = windowHeight - headerHeight - headerContent
    
    var minSliderHeight = $(".slides-content.active .content-box").outerHeight(true) + 30 + navWidth;

    if(minSliderHeight > totalHeight) {
        $(".slider-content").css("min-height", minSliderHeight);
    } else {
        $(".slider-content").css("min-height", totalHeight);
    }

    if($(window).width() <=480) {
        minSliderHeight = minSliderHeight + 75;
        $(".slider-content").css("min-height", minSliderHeight);
    } 
}

function placeholder(element) {
    var placeholderVal = $(element).attr('data-placeholder');

    if ($.trim($(element).val()) == "") {
        setTimeout(function(){
            $(element)
            .val(placeholderVal);
            /*.css({
            'color': '#75787E', 
            'opacity': '0.8'
            });*/
        },0)
    }

    $(element).unbind('click').bind('click', function() {
        if ($(element).val() === placeholderVal) {
            $(element)
           .val("");
           /*.css({
            'color': '', 
            'opacity': '1'
           });*/
        }
    });

    $(element).unbind('blur').bind('blur', function() {
         if ($.trim($(element).val()) == "") {
            $(element)
            .val(placeholderVal);
            /*.css({
            'color': '#75787E', 
            'opacity': '0.8'
            });*/
        }
    });
}