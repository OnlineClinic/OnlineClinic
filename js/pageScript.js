$(function() {


    var top = 1;
    $(window).scroll(function (event) {
        // what the y position of the scroll is
        var y = $(this).scrollTop();

        // whether that's below the form
        if (y >= top) {
            // if so, ad the fixed class
            $('.header').addClass('fixed');
        } else {
            // otherwise remove it
            $('.header').removeClass('fixed');
        }
    });
	  $('a[href*=#]:not([href=#])').click(function() {
	    if (location.pathname.replace(/^\//,'') == this.pathname.replace(/^\//,'') && location.hostname == this.hostname) {

	      var target = $(this.hash);
	      target = target.length ? target : $('[name=' + this.hash.slice(1) +']');
	      if (target.length) {
	        $('html,body').animate({
	          scrollTop: target.offset().top + -180
	        }, 1000);
	        return false;
	      }
	    }
	  });

   $(".nav-tabs a").click(function(){
        $(this).tab('show');
    });
	
//$('[data-toggle="tooltip"]').tooltip(); 

$('ul.slimmenu').slimmenu(
{
    resizeWidth: '700',
    collapserTitle: '',
    easingEffect:'easeInOutQuint',
    animSpeed:'medium',
    indentChildren: true,
    childrenIndenter: ''
});

$('.flexslider').flexslider({
    directionNav: true,
    animation: "slide",
    controlNav: false,
    slideshow: true,
	slideshowSpeed: 3000
});

$('.testimonial_slider').flexslider({
    directionNav: false,
    animation: "slide",
    controlNav: true,
    slideshow: false,
	slideshowSpeed: 3000
});

$(window).scroll(function () {
	    if ($(this).scrollTop() > 10) {
	        $('#toTop').fadeIn();
	    } else {
	        $('#toTop').fadeOut();
	    }
	});

	$('#toTop').click(function () {
	    $('body,html').animate({ scrollTop: 0 }, 800);
	});



var cmenuId = "." + $("#MenuId").text();
$(cmenuId).parent().addClass('active');

  });
  
  $('#owl-demo').owlCarousel({
    rtl:false,
    loop:true,
    nav:true,
	responsive:{
        0:{items:1},
        479:{items:2},
        766:{items:3},
		1024:{items:4},
		1280:{items:4}
    }
	
	
})


  
  
  

  





     