// Carrega o JSON e renderiza os filmes
async function carregarFilmes() {
    try {
        const response = await fetch('data/filmes.json');
        const filmes = await response.json();

        const moviesGrid = document.getElementById('movies-grid');
        moviesGrid.innerHTML = ''; // Limpa o grid antes de adicionar os filmes

        // Itera sobre os filmes (do mais recente ao mais antigo)
        filmes.reverse().forEach(filme => {
            // Cria o elemento do card do filme
            const movieCard = document.createElement('div');
            movieCard.className = 'movie-card';

            // Monta o HTML do card com link para a página de detalhes
            movieCard.innerHTML = `
                <a href="/filme/detalhes?id=${filme.id}">
                    <img src="${filme.capa}" alt="${filme.titulo}">
                    <h3>${filme.titulo}</h3>

                </a>
            `;

            // Adiciona o card ao grid
            moviesGrid.appendChild(movieCard);
        });
    } catch (error) {
        console.error('Erro ao carregar filmes:', error);
    }
}

// Carregar os filmes na inicialização
document.addEventListener('DOMContentLoaded', carregarFilmes);
