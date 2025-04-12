// categoria.js

let categorias = [];

function adicionarCategoria() {
    const select = document.getElementById("categoriaSelect");
    const categoria = select.value;

    if (!categoria || categorias.includes(categoria)) return;

    if (categorias.length >= 5) {
        alert("Você pode adicionar até 5 categorias.");
        return;
    }

    categorias.push(categoria);
    atualizarCategorias();
}

function removerCategoria(index) {
    categorias.splice(index, 1);
    atualizarCategorias();
}

function atualizarCategorias() {
    const container = document.getElementById("categoriasSelecionadas");
    container.innerHTML = "";

    categorias.forEach((cat, idx) => {
        const span = document.createElement("span");
        span.className = "badge";
        span.innerHTML = `${cat} <button type="button" onclick="removerCategoria(${idx})">&times;</button>`;
        container.appendChild(span);
    });

    document.getElementById("GeneroLista").value = categorias.join(", ");
}
