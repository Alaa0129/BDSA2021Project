
function changeTagColor(id) {

    var button = document.getElementById(id);
    var color = button.style.backgroundColor;
    color = button.style.backgroundColor = color === 'black' ? '#17a2b8' : 'black';
    
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




