const diretoresSugestoes = [
    "Kleber Mendonça Filho", "Anna Muylaert", "Fernando Meirelles", "Walter Salles",
    "José Padilha", "Cacá Diegues", "Carlos Saldanha", "Laís Bodanzky", "Heitor Dhalia",
    "Andrucha Waddington", "Daniel Filho", "José Mojica Marins", "Bruno Barreto",
    "Breno Silveira", "Jorge Furtado", "Tata Amaral", "Luiz Fernando Carvalho",
    "Guel Arraes", "Ruy Guerra", "Neville D’Almeida", "Gabriel Mascaro",
    "Helvécio Ratton", "Marcelo Gomes", "Claudio Assis", "Sandra Kogut",
    "Lucia Murat", "Selton Mello", "Monique Gardenberg", "Juliano Dornelles",
    "Karim Aïnouz", "Petra Costa", "Affonso Uchoa", "Glauber Rocha",
    "Eduardo Coutinho", "Arnaldo Jabor", "Hector Babenco", "Roberto Santos",
    "João Moreira Salles", "Adirley Queirós", "André Novais Oliveira",
    "Gabriel Martins", "Marília Rocha", "Ary Rosa", "Paulo César Saraceni",
    "Leon Hirszman", "Carlos Diegues", "Tizuka Yamasaki", "Sérgio Bianchi",
    "Júlio Bressane", "Andrea Tonacci", "Eduardo Escorel", "Suzana Amaral",
    "Roberto Farias", "Daniela Thomas", "Fellipe Barbosa", "Marcos Prado",
    "Eduardo Nunes", "Paulo Caldas", "Lírio Ferreira", "Sérgio Machado",
    "Eduardo Valente", "Julia Rezende", "Esmir Filho", "Gustavo Pizzi",
    "Aly Muritiba", "Lillah Halla", "Vladimir Carvalho", "Evaldo Mocarzel"
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
    "Zezé Motta", "Maurício Tizumba", "Otávio Müller", "Henri Castelli",
    "Mariana Ximenes", "Vera Fischer", "Antônio Fagundes", "Marcos Palmeira",
    "Carolina Dieckmann", "Erom Cordeiro", "Fábio Assunção", "Vladimir Brichta",
    "Sophie Charlotte", "Herson Capri", "Marcos Caruso", "Luís Melo",
    "Edson Celulari", "Ailton Graça", "Thalita Carauta", "Silvio Guindane",
    "Lee Taylor", "Nanda Costa", "Rosi Campos", "Gero Camilo",
    "Jonathan Haagensen", "Leticia Colin", "Isio Ghelman", "Aline Moraes",
    "Daniel de Oliveira", "Giovanna Antonelli", "Juliana Paes", "Humberto Martins",
    "Marcos Veras", "Débora Bloch", "Emílio Orciollo Netto", "André Ramiro",
    "Caco Ciocler", "Guilherme Weber", "César Troncoso", "Thiago Lacerda",
    "Bruno Gagliasso", "Marcello Novaes", "Cleo Pires", "Maria Flor",
    "Fiuk", "Sheron Menezzes", "Samya de Lavor", "Rafael Vitti",
    "Gabriel Leone", "Isabela Torres", "Romulo Braga", "Valentina Herszage"
];

// Ativação do Tagify para os campos
window.addEventListener("DOMContentLoaded", () => {
    const diretorInput = document.getElementById("DiretorLista");
    const elencoInput = document.getElementById("ElencoLista");

    if (diretorInput) {
        new Tagify(diretorInput, {
            whitelist: diretoresSugestoes,
            maxTags: 10,
            dropdown: {
                maxItems: 5,
                enabled: 0,
                closeOnSelect: false
            }
        });
    }

    if (elencoInput) {
        new Tagify(elencoInput, {
            whitelist: elencoSugestoes,
            maxTags: 30,
            dropdown: {
                maxItems: 5,
                enabled: 0,
                closeOnSelect: false
            }
        });
    }
});