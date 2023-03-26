function updateLight(currLight, classSelector){
    if (currLight === 1) {
        $(classSelector).addClass('off');
        $(classSelector).removeClass('on');
        $(classSelector).removeClass('ready');
    } else if (currLight === 2) {
        $(classSelector).addClass('ready');
        $(classSelector).removeClass('off');
        $(classSelector).removeClass('on');
    } else if (currLight === 3) {
        $(classSelector).addClass('on');
        $(classSelector).removeClass('off');
        $(classSelector).removeClass('ready');
    }
}

setInterval(function () {
    $.get("https://localhost:7003/Traffic", function (data, status) {
        console.log("Data: " + data + "\nStatus: " + status);
        var state = new Map();
        data.forEach(element => {
            state.set(element["trafficLightId"], element["currentState"]);
        });


        var stateFed = new Map();
        data.forEach(element => {
            stateFed.set(element["trafficLightId"], element["liveFeed"]);
        });
        //Side: First = 1, Second = 2, Third = 3, Fourth = 4
        //LightState: Stop = 1, Ready = 2, Go = 3
        //classes: on, off, ready
        
        //first side
        if (state.has(1)) {
            var currLight = state.get(1);
            updateLight(currLight, '.first')
            var currstateFed=stateFed.get(1);
            $("#light1").attr("src",currstateFed);
        }

        //second side
        if (state.has(2)) {
            var currLight = state.get(2);
           
            updateLight(currLight, '.second')
            var currstateFed=stateFed.get(2);
            $("#light2").attr("src",currstateFed);
        }

        //thirs side
        if (state.has(3)) {
            var currLight = state.get(3);
            updateLight(currLight, '.third')
            var currstateFed=stateFed.get(3);
            $("#light3").attr("src",currstateFed);
        }

        //fourth side
        if (state.has(4)) {
            var currLight = state.get(4);
            updateLight(currLight, '.fourth')
            var currstateFed=stateFed.get(4);
            $("#light4").attr("src",currstateFed);
        }
    });

}, 5000);
// Data: [{"LightState":1,"Side":1},{"LightState":1,"Side":2},{"LightState":3,"Side":3},{"LightState":1,"Side":4}]
// Status: success