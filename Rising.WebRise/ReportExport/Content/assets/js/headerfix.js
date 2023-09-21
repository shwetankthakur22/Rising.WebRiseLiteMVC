(function($) {
    "use strict";
	
	//header-fix
	$(window).on("scroll", function(e){
		if ($(window).scrollTop() >= 150) {
			$('.Klast-navbar').addClass('fixed-header');
			$('.Klast-navbar').addClass('visible-title');
		}
		else {
			$('.Klast-navbar').removeClass('fixed-header');
			$('.Klast-navbar').removeClass('visible-title');
		}
    });
})(jQuery);	