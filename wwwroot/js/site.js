function validarLogin(){

    let usuario= document.getElementById("nombreUsuario").value;
    let contraseña= document.getElementById("contraseña").value;
    let mensaje= document.getElementById("mensaje");

    if (usuario == ""){
        mensaje.innerHTML = "Complete el usuario";
        return false;
    }

    if (contraseña == ""){
        mensaje.innerHTML= "Complete la contraseña";
        return false;
    }

    if (usuario.length <4){
        mensaje.innerHTML= "El usuario debe tener como mínimo 4 caracteres";
        return false;
    }

    if (contraseña.length< 4){
        mensaje.innerHTML="La contraseña debe tener como mínimo 4 caracteres";
        return false;
    }

    return true;
}


function validarRegistro(){

    let usuario=document.getElementById("nombreUsuario").value;
    let contraseña =document.getElementById("contraseña").value;
    let nombre= document.getElementById("nombre").value;
    let apellido=document.getElementById("apellido").value;
    let tipoUsuario= document.getElementById("tipousuario").value;
    let mensaje= document.getElementById("mensaje");

    if (usuario == ""){
        mensaje.innerHTML= "Complete el usuario";
        return false;
    }

    if (contraseña == ""){
        mensaje.innerHTML= "Complete la contraseña";
        return false;
    }
    if (nombre==""){
        mensaje.innerHTML="Complete el nombre";
        return false;
    }
    if (apellido==""){
        mensaje.innerHTML= "Complete el apellido";
        return false;
    }
    if (tipoUsuario== ""){
        mensaje.innerHTML= "Seleccione un tipo de usuario";
        return false;
    }
    if (usuario.length<4){
        mensaje.innerHTML="El usuario debe tener como minimo 4 caracteres";
        return false;
    }
    if (contraseña.length<4){
        mensaje.innerHTML= "La contraseña debe tener como minimo 4 caracteres";
        return false;
    }
    if(nombreValido(nombre)== false){
        mensaje.innerHTML="El nombre solo puede contener letras y espacios";
        return false;
    }
    if (nombreValido(apellido)== false){
        mensaje.innerHTML= "El apellido solo puede contener letras y espacios";
        return false;
    }
    return true;
}

function nombreValido(texto){

    for (let i =0;i< texto.length;i++) {

        let caracter = texto[i];

        if ((caracter< "A" || caracter> "Z") &&(caracter< "a" || caracter> "z") && caracter != " ") {

            return false;
        }
    }

    return true;
}