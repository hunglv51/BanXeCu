var carName = document.getElementById("CarName");
var family = document.getElementById("Family");
carName.onchange = selectList;
function selectList() {
    var nameSelect = carName.value;
    switch (nameSelect) {
        case "BMW":
            insertSelectTag(bmwCar);
            break;
        case "Audi":
            insertSelectTag(audiCar);
            break;
        case "Chevrolet":
            insertSelectTag(chevroletCar);
            break;
        case "Daewoo":
            insertSelectTag(daewooCar);
            break;
        case "Ford":
            insertSelectTag(fordCar);
            break;
        case "Honda":
            insertSelectTag(hondaCar);
            break;
        case "Huyndai":
            insertSelectTag(huyndaiCar);
            break;
        case "Isuzu":
            insertSelectTag(isuzuCar);
            break;
        case "Jaguar":
            insertSelectTag(jaguarCar);
            break;
        case "Kia":
            insertSelectTag(kiaCar);
            break;
        case "Land Rover":
            insertSelectTag(landroverCar);
            break;
        case "Lexus":
            insertSelectTag(lexusCar);
            break;
        case "Madza":
            insertSelectTag(madzaCar);
            break;
        case "Mercedes-Benz":
            insertSelectTag(mercedesCar);
            break;
        case "Mitsubishi":
            insertSelectTag(mitsubishiCar);
            break;
        case "Nissan":
            insertSelectTag(nissanCar);
            break;
        case "Peugeot":
            insertSelectTag(peugeotCar);
            break;
        case "Suzuki":
            insertSelectTag(suzukiCar);
            break;
        case "Toyota":
            insertSelectTag(toyotaCar);
            break;
        default:
            insertSelectTag(volkswagenCar);
            break;
    }
}
function insertSelectTag(array) {
    var element = "";
    for (var item in array) {
        element += "<option id = '" + array[item] + "'>" + array[item] + "</option>";
    }
    family.innerHTML = element;
}
function setSelectedChoice(idOptTag) {
    optTag = document.getElementById(idOptTag);
    optTag.setAttribute("selected", "selected");
}
