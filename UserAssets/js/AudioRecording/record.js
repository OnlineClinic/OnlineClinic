//http: //stackoverflow.com/questions/2988050/html5-audio-player-jquery-toggle-click-play-pause

function restore() { $(".btnRecord,#live").removeClass("disabled"); $(".one").addClass("disabled"); $.voice.stop(); }
$(document).ready(function ()
{
    $(document).on("click", ".btnRecord", function ()
    {
        var btnRecordId = this.id;
        var dvId = btnRecordId.replace('record', 'dvCounter');
        var btnPlayId = btnRecordId.replace('record', 'play');
        elem = $(this);
        startTimer(dvId, btnPlayId);

        $.voice.record($("#live").is(":checked"), function ()
        {
            elem.addClass("disabled");
            $("#live").addClass("disabled");
            $("#stop,#play,#download").removeClass("disabled");
        });
    });
    $(document).on("click", ".btnStop", function ()
    {
        var btnStopId = this.id;
        var audioPlayerId = btnStopId.replace('stop', 'audio');
        var IsPlayId = btnStopId.replace('stop', 'IsPlay');
       
        $("#" + audioPlayerId)[0].pause();
        $("#" + audioPlayerId)[0].currentTime = 0;
        document.getElementById(IsPlayId).value = '0';
        // restore();
    });
    $(document).on("click", ".btnPlay", function ()
    {
        var btnPlayId = this.id;
        var audioPlayerId = btnPlayId.replace('play', 'audio');

        $.voice.export(function (url)
        {
            $("#" + audioPlayerId).attr("src", url);
            getFileName(url, audioPlayerId);
            //$("#" + audioPlayerId)[0].play();

        }, "URL");
       // restore();
    });
    $(document).on("click", "#download:not(.disabled)", function ()
    {
        $.voice.export(function (url)
        {
            $("<a href='" + url + "' download='MyRecording.wav'></a>")[0].click();
        }, "URL");
        restore();
    });
});