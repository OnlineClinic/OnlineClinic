/**
 * Infinite Carousel Slider
 * developed by Sanjay Verma
 * Github Repo https://github.com/sanjayV/infinite_Image_Carousel
 */
(function( $ ) {
	$.fn.carousel = function(options) {

		// default settings
        var settings = $.extend({
            auto         : false,
            animateSpeed : 10000,
            autoSpeed    : 5000,
            isPagination : true
        }, options);

		var currentImg = 0
			, newImg = 0
			, totalImg = 0
			, mainClass = this
			, innerClass = '.slides'
			, innerContentClass = '.slides-content'
			, carouselWidth = 0
			, animateSpeed = settings.animateSpeed
			, autoSpeed = settings.autoSpeed
			, isComplete = true
			, currentInterval = ""
			, paginationDiv = ".slider-tabbing .container";

		init = function() {
			totalImg = $(mainClass).find(innerClass).length;
			getCarouselPosition();
			bindEvents();

			if (settings.auto) {
				autoSlide();
			}

			if (settings.isPagination) {
				setPagination();
			}
		};

		bindEvents = function() {

			$( window ).resize(function() {
				carouselWidth = $(mainClass).find(innerClass).width();
			});

			$('.prev').unbind('click').bind('click', function(e) {
				e.preventDefault();
				newImg = currentImg - 1;

				if (newImg < 0) {
					newImg = totalImg - 1;
				}

				if (isComplete){
                    slideCarousel('prev');
                    imageCover();
                }
			});

			$('.next').unbind('click').bind('click', function(e) {
				e.preventDefault();
				newImg = currentImg + 1;

				if (newImg > totalImg - 1) {
					newImg = 0;
				}

				if (isComplete){
                    slideCarousel('next');
                    imageCover();
                }
			});

			$('.slider-control span').hover(function()  {
				if (currentInterval != "")
					clearInterval(currentInterval);
			}, function() {
				autoSlide();
			})
		};

		getCarouselPosition = function() {
			carouselWidth = $(mainClass).find(innerClass).width();
		};

		slideCarousel = function(direction) {
			isComplete = false;
			var newImgMove = carouselWidth+'px'
				, oldImgMove = -1*(carouselWidth) + 'px';


			if (direction == 'prev') {
				newImgMove = -1*(carouselWidth) + 'px';
				oldImgMove = carouselWidth + 'px';
			}

			$(mainClass).find(innerClass).removeClass('active');
            $(mainClass).find(innerClass).eq(currentImg).removeClass('active');
            $(mainClass).find(innerClass).eq(currentImg).fadeOut(1000);

			$(mainClass).find(innerClass).eq(newImg).fadeIn(1000, function() {
                //isComplete = true;
            });
			$(mainClass).find(innerClass).eq(newImg).addClass('active');

			/* make active navigation tab's */
			if (settings.isPagination) {
				$(paginationDiv).find('ul li').removeClass('active');
				$(paginationDiv).find('ul li').eq(newImg).addClass('active');
			}

            /*currentImg = newImg;*/

            /*Slider content*/
            var sliderContent = $(mainClass).parent('.slider').next('.carousel').find('.slider-content');

			//console.log(sliderContent);
			$(sliderContent).find(innerContentClass).eq(newImg).css("left",newImgMove);
			
			$(sliderContent).find(innerContentClass).eq(newImg).animate({
				left: 0
			}, animateSpeed, function() {
				$(sliderContent).find(innerContentClass).eq(newImg).addClass('active');
			});


			//For set height as per content
			var contentHeight = '';
			contentHeight = $(sliderContent).find(innerContentClass).eq(newImg).addClass('active').height();  
			$(sliderContent).height(contentHeight);
			//For set height as per content

			//$(sliderContent).find(innerContentClass).eq(newImg).addClass('active');

			$(sliderContent).find(innerContentClass).eq(currentImg).animate({
				left: oldImgMove
			}, animateSpeed, function() {
				$(this).attr('style', "");
				$(sliderContent).find(innerContentClass).not(':eq('+newImg+')').removeClass('active');

				currentImg = newImg;

				isComplete = true;
			});

 			/*Slider content*/
		};

		setPagination = function() {
			if (totalImg) {
				paginationEvent();
			}
		};

		paginationEvent = function() {
			if (settings.isPagination && $(paginationDiv).find('ul').length) {
				$(paginationDiv).find('ul li').unbind('click').bind('click', function() {
					if ($(this).attr('data-index') && isComplete) {
						newImg = parseInt($(this).attr('data-index'));
						
						if (newImg > currentImg)
							slideCarousel('next');
						else if (newImg < currentImg)
							slideCarousel('prev');
					}
				});
			}
		};

		autoSlide = function() {

			/*currentInterval = setInterval(function() {
				newImg = currentImg + 1;

				if (newImg > totalImg - 1) {
					newImg = 0;
				}

				slideCarousel('next');
			}, autoSpeed);*/
		};

		init();

	};

}( jQuery ));