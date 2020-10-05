'use strict';
var connection=new signalR.HubConnectionBuilder().withUrl("/chat").build();
connection.on("ReceiveMessage",function(message){
    var msg=message.replace(/&/g, "&amp;").replace(/</g, "&lt;").replace(/>/g, "&gt;");
    if(msg!="")
    {
        var p=document.createElement("p");
        p.textContent=msg;
        document.getElementById("messages-from-other").appendChild(p);
        var messageBlock=document.getElementById("messages-from-other");
        messageBlock.scrollTop=messageBlock.scrollHeight*2;
    }
    
});
connection.start();
document.getElementsByTagName("button")[0].addEventListener("click",function(event){
    var message=document.getElementById("input-area").value;
    connection.invoke("Send",message).catch(function(err){
        return console.error(err.toString());
    });
   
});
document.getElementsByTagName("button")[1].addEventListener("click",function(event){
    document.getElementById("input-area").value="";
});

