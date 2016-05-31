
var wofrids = [
                "#waveform0", "#waveform1", "#waveform2",
                "#waveform3", "#waveform4", "#waveform5",
                "#waveform6", "#waveform7", "#waveform8",
                "#waveform9", "#waveform10", "#waveform11",
                "#waveform12", "#waveform13", "#waveform14",
                "#waveform15", "#waveform16", "#waveform17",
                "#waveform18", "#waveform19"
              ];

// Create an instance


function getstarobt(j, chk)
{
    try {

        if (!chk) {
            obj = null;
            micro = null;

            blobs = [];

            obj = Object.create(WaveSurfer);
            var options = {
                container: "#waveform" + j,
                waveColor: 'green',
                interact: true,
                cursorWidth: 0
            };

            obj.init(options);
            micro = Object.create(WaveSurfer.Microphone);

            micro.init({
                wavesurfer: obj
            });

            micro.on('deviceReady', function (stream)
            {

                var con = { 'stream': stream, 'onRecordedMedia': ghu };
                AudioVideoRecorder(con);
                console.info('Device ready!');
                startTimer('dvCounter' + j.toString(), '');

            });

            micro.on('ready', function (code)
            {
                console.warn('vvvDevice error: ' + code);
            });


            micro.on('deviceError', function (code)
            {
                console.warn('Device error: ' + code);
            });

            micro.start();

            $("button[id^=\"micBtn\"]").prop('disabled', true); ;
            $("#micBtn_" + j).prop('disabled', false);

            return true;

        } else {

            micro.stop();
            sendDataToServer();
            $("#waveform" + j).html('');
            $("button[id^=\"micBtn\"]").prop('disabled', false);

        }

    } catch (e) {

        return false;
    }

    return false;
}


