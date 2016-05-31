include('jquery.min.js');
include('bootstrap.js');
include('jquery.flexslider.js');
include('jquery.easing.min.js');
include('jquery.slimmenu.js');
include('owl.carousel.js');
include('jquery.unveilEffects.js');
include('css_browser_selector.js');
include('featherlight.min.js');
include('pageScript.js');
//----Include-Function----
function include(url) {
    document.write('<script type="text/javascript" src="/js/' + url + '"></script>');
    return false;
}

