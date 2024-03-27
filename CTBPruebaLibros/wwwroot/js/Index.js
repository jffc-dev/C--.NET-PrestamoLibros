const _modeloLibro = {
    id: 0,
    titulo: "",
    autor: "",
    descripcion: "",
    fechaPublicacion: "",
    genero: "",
    imagen: "",
    cantidad: 0
};

function MostrarLibros() {

    fetch("/Home/ListarLibros")
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            console.log(responseJson)
            if (responseJson.length > 0) {

                $("#tablaLibros tbody").html("");


                responseJson.forEach((libro) => {
                    $("#tablaLibros tbody").append(
                        $("<tr>").append(
                            $("<td>").append('<img width="60px" src="' + libro.imagen + '" />'),
                            $("<td>").text(libro.titulo),
                            $("<td>").text(libro.autor),
                            $("<td>").text(libro.descripcion),
                            $("<td>").text(DateAString(libro.fechaPublicacion)),
                            $("<td>").text(libro.genero),
                            $("<td>").text(libro.cantidad),
                            $("<td class='d-flex flex-column justify-content-center'>").append(
                                $("<button>").addClass("btn btn-primary btn-sm boton-editar-libro").data("dataLibro", libro).text("Editar"),
                                $("<button>").addClass("btn btn-danger btn-sm boton-eliminar-libro").data("dataLibro", libro).text("Eliminar"),
                                $("<button>").addClass("btn btn-success btn-sm boton-prestar-libro").data("dataLibro", libro).text("Prestar").prop("disabled", libro.cantidad == 0)
                            )
                        )
                    )
                })

            }


        })


}


document.addEventListener("DOMContentLoaded", function () {

    MostrarLibros();

    $("#txtFechaPublicacion").datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayHighlight: true
    })

    $("#txtFechaSalida").datepicker({
        format: "dd/mm/yyyy",
        autoclose: true,
        todayHighlight: true
    })


}, false)


function MostrarModalLibro() {
    $("#txtTitulo").val(_modeloLibro.titulo);
    $("#txtAutor").val(_modeloLibro.autor);
    $("#txtDescripcion").val(_modeloLibro.descripcion);
    $("#txtFechaPublicacion").val(_modeloLibro.fechaPublicacion);
    $("#txtGenero").val(_modeloLibro.genero);
    $("#txtImagen").val(_modeloLibro.imagen);
    $("#txtCantidad").val(_modeloLibro.cantidad);


    $("#modalLibro").modal("show");

}

function MostrarModalPrestamo() {
    $("#txtTituloPrestamo").val(_modeloLibro.titulo);
    $("#txtFechaSalida").val("");


    $("#modalPrestamo").modal("show");

}

function StringADate(fechaString) {
    const partes = fechaString.split('/');
    const dia = parseInt(partes[0], 10);
    const mes = parseInt(partes[1], 10) - 1; 
    const anio = parseInt(partes[2], 10);

    const fecha = new Date(anio, mes, dia);

    return fecha
}

function DateAString(fecha) {
    const fechaDate = new Date(fecha)
    const dia = fechaDate.getDate();
    const mes = fechaDate.getMonth() + 1;
    const anio = fechaDate.getFullYear();
    const diaFormateado = (dia < 10) ? '0' + dia : dia;
    const mesFormateado = (mes < 10) ? '0' + mes : mes;

    const fechaFormateada = `${diaFormateado}/${mesFormateado}/${anio}`;

    return fechaFormateada
}

$(document).on("click", ".boton-nuevo-libro", function () {

    _modeloLibro.idLibro = 0;
    _modeloLibro.nombreCompleto = "";
    _modeloLibro.idDepartamento = 0;
    _modeloLibro.sueldo = 0;
    _modeloLibro.fechaContrato = "";

    MostrarModalLibro();

})

$(document).on("click", ".boton-editar-libro", function () {

    const _libro = $(this).data("dataLibro");


    _modeloLibro.id = _libro.id;
    _modeloLibro.titulo = _libro.titulo;
    _modeloLibro.autor = _libro.autor;
    _modeloLibro.descripcion = _libro.descripcion;
    _modeloLibro.fechaPublicacion = _libro.fechaPublicacion;
    _modeloLibro.genero = _libro.genero;
    _modeloLibro.imagen = _libro.imagen;
    _modeloLibro.cantidad = _libro.cantidad;

    MostrarModalLibro();

})

$(document).on("click", ".boton-prestar-libro", function () {

    const _libro = $(this).data("dataLibro");


    _modeloLibro.id = _libro.id;
    _modeloLibro.titulo = _libro.titulo;
    _modeloLibro.autor = _libro.autor;
    _modeloLibro.descripcion = _libro.descripcion;
    _modeloLibro.fechaPublicacion = _libro.fechaPublicacion;
    _modeloLibro.genero = _libro.genero;
    _modeloLibro.imagen = _libro.imagen;
    _modeloLibro.cantidad = _libro.cantidad;

    MostrarModalPrestamo();

})

$(document).on("click", ".boton-guardar-cambios-libro", function () {
    console.log($("#txtFechaPublicacion").val())
    const libro = {
        id: _modeloLibro.id,
        titulo: $("#txtTitulo").val(),
        autor: $("#txtAutor").val(),
        descripcion: $("#txtDescripcion").val(),
        fechaPublicacion: new Date(),
        genero: $("#txtGenero").val(),
        imagen: $("#txtImagen").val(),
        cantidad: $("#txtCantidad").val()
    };

    console.log(libro)


    if (_modeloLibro.idLibro == 0) {

        fetch("/Home/GuardarLibro", {
            method: "POST",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(libro)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {

                if (responseJson.resultado) {
                    $("#modalLibro").modal("hide");
                    Swal.fire("Listo!", "Libro fue creado", "success");
                    MostrarLibros();
                }
                else
                    Swal.fire("Lo sentimos", "No se puedo crear", "error");
            })

    } else {

        fetch("/Home/EditarLibro", {
            method: "PUT",
            headers: { "Content-Type": "application/json; charset=utf-8" },
            body: JSON.stringify(libro)
        })
            .then(response => {
                return response.ok ? response.json() : Promise.reject(response)
            })
            .then(responseJson => {

                if (responseJson.resultado) {
                    $("#modalLibro").modal("hide");
                    Swal.fire("Listo!", "El libro fue actualizado", "success");
                    MostrarLibros();
                }
                else
                    Swal.fire("Lo sentimos", "No se puedo actualizar", "error");
            })

    }


})

$(document).on("click", ".boton-guardar-cambios-prestamo", function () {

    const prestamo = {
        id: _modeloLibro.id,
        fechaSalida: StringADate($("#txtFechaSalida").val())
    };


    fetch("/Home/PrestarLibro", {
        method: "POST",
        headers: { "Content-Type": "application/json; charset=utf-8" },
        body: JSON.stringify(prestamo)
    })
        .then(response => {
            return response.ok ? response.json() : Promise.reject(response)
        })
        .then(responseJson => {
            console.log(responseJson)
            if (responseJson.resultado) {
                $("#modalPrestamo").modal("hide");
                Swal.fire("Listo!", "El libro fue prestado", "success");
                MostrarLibros();
            }
            else
                Swal.fire("Lo sentimos", "No se puedo prestar el libro", "error");
        })


})


$(document).on("click", ".boton-eliminar-libro", function () {

    const _libro = $(this).data("dataLibro");

    console.log(_libro)

    Swal.fire({
        title: "Está seguro que desea eliminar el libro?",
        text: `Eliminar libro "${_libro.titulo}"`,
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Si, eliminar",
        cancelButtonText: "No, volver"
    }).then((result) => {

        if (result.isConfirmed) {

            fetch(`/Home/EliminarLibro?idLibro=${_libro.id}`, {
                method: "DELETE"
            })
                .then(response => {
                    return response.ok ? response.json() : Promise.reject(response)
                })
                .then(responseJson => {

                    if (responseJson.resultado) {
                        Swal.fire("Listo!", "El libro fue elminado", "success");
                        MostrarLibros();
                    }
                    else
                        Swal.fire("Lo sentimos", "No se puedo eliminar", "error");
                })

        }



    })

})