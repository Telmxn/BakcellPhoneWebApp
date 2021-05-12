function menufunc(_this) {
    var menu = document.getElementById("menu");
    menu.classList.toggle("act");
    _this.classList.toggle("act");
  }

function confirmFunction() {
    confirm("İstifadəçi silinsin?");
}

function dropdwn() {
    var drpdwn = document.getElementById("drpcnt");
    drpdwn.classList.toggle("drpdwn-show");
}