let desde = 10;

async function cambiarLike(idPublicacion, boton) {
    const datos = new URLSearchParams();
    datos.append("idPublicacion", idPublicacion);

    const respuesta = await fetch("/Publicacion/CambiarLike", {
        method: "POST",
        headers: { "Content-Type": "application/x-www-form-urlencoded" },
        body: datos
    });

    const resultado = await respuesta.json();

    if (!resultado.ok) {
        alert(resultado.mensaje);
        return;
    }

    boton.querySelector(".cantidad-likes").innerText = resultado.cantidadLikes;

    if (resultado.dioLike) {
        boton.classList.remove("btn-outline-danger");
        boton.classList.add("btn-danger");
    } else {
        boton.classList.remove("btn-danger");
        boton.classList.add("btn-outline-danger");
    }
}

async function comentar(idPublicacion, boton) {
    const publicacion = boton.closest(".publicacion");
    const input = publicacion.querySelector(".comentario-texto");
    const mensaje = publicacion.querySelector(".mensaje-comentario");
    const texto = input.value.trim();

    mensaje.innerText = "";

    if (texto === "") {
        mensaje.innerText = "El comentario no puede estar vacío";
        return;
    }

    const datos = new URLSearchParams();
    datos.append("idPublicacion", idPublicacion);
    datos.append("texto", texto);

    const respuesta = await fetch("/Publicacion/Comentar", {
        method: "POST",
        headers: { "Content-Type": "application/x-www-form-urlencoded" },
        body: datos
    });

    const resultado = await respuesta.json();

    if (!resultado.ok) {
        mensaje.innerText = resultado.mensaje;
        return;
    }

    const comentario = document.createElement("div");
    comentario.className = "comentario";

    const usuario = document.createElement("strong");
    usuario.innerText = resultado.nombreUsuario;

    const textoComentario = document.createElement("span");
    textoComentario.innerText = resultado.texto;

    const fecha = document.createElement("small");
    fecha.innerText = resultado.fechaComentario;

    comentario.appendChild(usuario);
    comentario.appendChild(textoComentario);
    comentario.appendChild(fecha);

    publicacion.querySelector(".comentarios").appendChild(comentario);
    input.value = "";
}

async function verMas() {
    const boton = document.getElementById("btnVerMas");
    boton.disabled = true;
    boton.innerText = "Cargando...";

    const respuesta = await fetch("/Publicacion/ObtenerMas?desde=" + desde);
    const resultado = await respuesta.json();

    if (!resultado.ok) {
        alert(resultado.mensaje);
        boton.disabled = false;
        boton.innerText = "Ver más";
        return;
    }

    for (const publicacion of resultado.publicaciones) {
        agregarPublicacion(publicacion);
    }

    desde += 10;

    if (resultado.publicaciones.length < 10) {
        boton.style.display = "none";
    } else {
        boton.disabled = false;
        boton.innerText = "Ver más";
    }
}

function agregarPublicacion(publicacion) {
    const contenedor = document.getElementById("publicaciones");
    const article = document.createElement("article");
    article.className = "card mb-4 shadow-sm publicacion";
    article.dataset.id = publicacion.idPublicacion;

    const imagen = document.createElement("img");
    imagen.className = "publicacion-imagen";
    imagen.src = publicacion.imagen;
    imagen.alt = "Imagen de la publicación";

    const body = document.createElement("div");
    body.className = "card-body";

    const titulo = document.createElement("h2");
    titulo.innerText = publicacion.titulo;

    const info = document.createElement("p");
    info.className = "text-muted";
    info.innerText = "Publicado por " + publicacion.nombreUsuario + " el " + formatearFecha(publicacion.fechaPublicacion);

    const descripcion = document.createElement("p");
    descripcion.innerText = publicacion.descripcion;

    const like = document.createElement("button");
    like.className = "btn " + (publicacion.usuarioDioLike ? "btn-danger" : "btn-outline-danger") + " btn-like";
    like.innerHTML = "❤ Me gusta (<span class='cantidad-likes'>" + publicacion.cantidadLikes + "</span>)";
    like.onclick = function () { cambiarLike(publicacion.idPublicacion, like); };

    const hr = document.createElement("hr");
    const subtitulo = document.createElement("h5");
    subtitulo.innerText = "Comentarios";

    const comentarios = document.createElement("div");
    comentarios.className = "comentarios";

    for (const comentario of publicacion.comentarios) {
        agregarComentario(comentarios, comentario);
    }

    const grupo = document.createElement("div");
    grupo.className = "input-group mt-3";

    const input = document.createElement("input");
    input.type = "text";
    input.className = "form-control comentario-texto";
    input.placeholder = "Escribí un comentario...";

    const botonComentar = document.createElement("button");
    botonComentar.className = "btn btn-primary";
    botonComentar.innerText = "Comentar";
    botonComentar.onclick = function () { comentar(publicacion.idPublicacion, botonComentar); };

    const mensaje = document.createElement("p");
    mensaje.className = "mensaje-comentario text-danger mt-2";

    grupo.appendChild(input);
    grupo.appendChild(botonComentar);
    body.appendChild(titulo);
    body.appendChild(info);
    body.appendChild(descripcion);
    body.appendChild(like);
    body.appendChild(hr);
    body.appendChild(subtitulo);
    body.appendChild(comentarios);
    body.appendChild(grupo);
    body.appendChild(mensaje);
    article.appendChild(imagen);
    article.appendChild(body);
    contenedor.appendChild(article);
}

function agregarComentario(contenedor, comentario) {
    const div = document.createElement("div");
    div.className = "comentario";

    const usuario = document.createElement("strong");
    usuario.innerText = comentario.nombreUsuario;
    const texto = document.createElement("span");
    texto.innerText = comentario.texto;
    const fecha = document.createElement("small");
    fecha.innerText = formatearFecha(comentario.fechaComentario);

    div.appendChild(usuario);
    div.appendChild(texto);
    div.appendChild(fecha);
    contenedor.appendChild(div);
}

function formatearFecha(fecha) {
    const date = new Date(fecha);
    if (isNaN(date.getTime())) return fecha;
    return date.toLocaleString("es-AR", { day: "2-digit", month: "2-digit", year: "numeric", hour: "2-digit", minute: "2-digit" });
}
