const imageSelector = document.getElementById("imageSelector")
imageSelector.addEventListener("change", function () {
	var fileName = this.files[0].name;
	var fileSize = this.files[0].size;
	var esArchValido = true;

	if (fileSize > 3000000) {
		alert('El archivo no debe superar los 3MB');
		this.value = '';
		this.files[0].name = '';
		esArchValido = false;
	} else {
		// recuperamos la extensión del archivo
		var ext = fileName.split('.').pop();

		// Convertimos en minúscula
		ext = ext.toLowerCase();

		switch (ext) {
			case 'jpg':
			case 'jpeg':
			case 'png': break;
			default:
				alert('El archivo no tiene la extensión adecuada');
				this.value = ''; // reset del valor
				this.files[0].name = '';
				esArchValido = false;
		}
	}

	if (esArchValido) {
		readURL(this);
	}
})

function readURL(input) {
    if (input.files && input.files[0]) {
        var reader = new FileReader();

		reader.onload = function (e) {

			const image = document.getElementById("image");

			image.setAttribute("src", e.target.result);
			image.setAttribute("display", "block");
			image.setAttribute("style", "display:block");
        }

        reader.readAsDataURL(input.files[0]); // leer el archivo string base64
    }
}