
function changeTagColor(id) {

    var button = document.getElementById(id);
    var bgColor = button.style.backgroundColor;
    bgColor = button.style.backgroundColor = bgColor === 'black' ? '#1861ac' : 'black';
    
}

//Clears the textfield for adding tags
function clearTagTextField() {

    document.getElementById("AddTagTextField").value = "";
}


//Clears each field for the create project form
function clearProjectForm() {

    document.getElementById("formGroupExampleInput1").value = "";
    document.getElementById("formGroupExampleInput2").value = "";
}




