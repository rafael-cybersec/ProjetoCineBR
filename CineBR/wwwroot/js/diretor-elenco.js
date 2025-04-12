const diretoresSugestoes = [
    "Kleber Mendonça Filho", "Anna Muylaert", "Fernando Meirelles", "Walter Salles",
    "José Padilha", "Cacá Diegues", "Carlos Saldanha", "Laís Bodanzky", "Heitor Dhalia",
    "Andrucha Waddington", "Daniel Filho", "José Mojica Marins", "Bruno Barreto",
    "Breno Silveira", "Jorge Furtado", "Tata Amaral", "Luiz Fernando Carvalho",
    "Guel Arraes", "Ruy Guerra", "Neville D’Almeida", "Gabriel Mascaro",
    "Helvécio Ratton", "Marcelo Gomes", "Claudio Assis", "Sandra Kogut",
    "Lucia Murat", "Selton Mello", "Monique Gardenberg", "Juliano Dornelles"
];

const elencoSugestoes = [
    "Wagner Moura", "Alice Braga", "Lázaro Ramos", "Sônia Braga", "Selton Mello",
    "Rodrigo Santoro", "Débora Falabella", "Leandra Leal", "Fernanda Montenegro",
    "Matheus Nachtergaele", "Marco Nanini", "Glória Pires", "Tony Ramos",
    "Camila Pitanga", "Marieta Severo", "Chico Diaz", "Murilo Benício",
    "Cauã Reymond", "Patrícia Pillar", "Paolla Oliveira", "Cássia Kis",
    "Renata Sorrah", "Letícia Sabatella", "Irandhir Santos", "Juliano Cazarré",
    "Bruna Linzmeyer", "Gregório Duvivier", "Karine Teles", "Jesuíta Barbosa",
    "Tainá Müller", "Maeve Jinkings", "Dira Paes", "João Miguel",
    "Sérgio Mamberti", "Taís Araújo", "Caio Blat", "Milhem Cortaz",
    "Johnny Massaro", "Clarisse Abujamra", "José de Abreu", "Lima Duarte",
    "Zezé Motta", "Maurício Tizumba", "Otávio Müller", "Henri Castelli"
];

// Ativação do Tagify para os campos
window.addEventListener("DOMContentLoaded", () => {
    const diretorInput = document.getElementById("DiretorLista");
    const elencoInput = document.getElementById("ElencoLista");

    if (diretorInput) {
        new Tagify(diretorInput, {
            whitelist: diretoresSugestoes,
            maxTags: 5,
            dropdown: {
                maxItems: 15,
                enabled: 0,
                closeOnSelect: false
            }
        });
    }

    if (elencoInput) {
        new Tagify(elencoInput, {
            whitelist: elencoSugestoes,
            maxTags: 10,
            dropdown: {
                maxItems: 20,
                enabled: 0,
                closeOnSelect: false
            }
        });
    }
});