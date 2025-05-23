const input = document.getElementById('search-input');
const container = document.getElementById('resultadosPesquisa');

// Normalização para ignorar acentos e caracteres especiais
const normalizarTexto = (texto) => {
    return texto.normalize('NFD')
        .replace(/[\u0300-\u036f]/g, '') // remove acentos
        .replace(/[^\w\s]/g, '')         // mantém letras, números e espaços
        .toLowerCase()
        .trim();
};

// Mostrar resultados com redirecionamento
const mostrarResultados = (data) => {
    container.innerHTML = '';
    container.style.display = 'block';

    if (!data || data.length === 0) {
        container.innerHTML = '<div class="resultado-item">Nenhum filme encontrado.</div>';
        return;
    }

    data.forEach(filme => {
        const item = document.createElement('div');
        item.classList.add('resultado-item');

        // Corrigido: usando `filme.titulo` e `filme.genero`
        item.innerHTML = `
            <div>
                <strong>${filme.titulo}</strong>
                <small>${(filme.genero || []).join(' | ')}</small>
            </div>
        `;

        item.addEventListener('click', () => {
            if (filme && filme.id) {
                window.location.href = `/filme/detalhes/${filme.id}`;
            } else {
                console.error('ID do filme não encontrado:', filme);
            }
        });

        container.appendChild(item);
    });
};

// Evento de input com fetch corrigido
input.addEventListener('input', function () {
    const termo = normalizarTexto(this.value);

    if (termo.length < 2) {
        container.innerHTML = '';
        container.style.display = 'none';
        return;
    }

    fetch(`/Filmes/Buscar?termo=${encodeURIComponent(termo)}`)
        .then(response => response.json())
        .then(data => mostrarResultados(data))
        .catch(err => {
            console.error("Erro ao buscar:", err);
            container.innerHTML = '';
            container.style.display = 'none';
        });
});

// Fechar dropdown ao clicar fora
document.addEventListener('click', function (e) {
    if (!document.querySelector('.search-container').contains(e.target)) {
        container.innerHTML = '';
        container.style.display = 'none';
    }
});
