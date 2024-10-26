const app = Vue.createApp({
    data() {
        return {
            gameTables: [],
            isFirstAidButtonPressed : false
        }
    },
    mounted() {
        this.refreshData()
    },
    methods: {
        refreshData() {
            axios.get("https://localhost:7027/api/GameTables")
                .then((response) => {
                    this.gameTables = response.data.map(table => ({
                        ...table,
                        isFirstAidButtonPressed: false,
                        isMouthBasketButtonPressed : false,
                        isMinusButtonPressed: false
                    }));
                })
                .catch((error) => {
                    console.error("Error during API call: ", error);
                });
        },
        formatDate(dateString) {
            const options = { year: 'numeric', month: '2-digit', day: '2-digit', hour: '2-digit', minute: '2-digit' };
            const date = new Date(dateString);
            return date.toLocaleString('hu-HU', options);
        },
        toggleFirstAidButton(tableId) {
            const table = this.gameTables.find(t => t.tableId === tableId);
            if (table) {
                table.isFirstAidButtonPressed = !table.isFirstAidButtonPressed;
            }
        },
        toggleMouthBasketButton(tableId) {
            const table = this.gameTables.find(t => t.tableId === tableId);
            if (table) {
                table.isMouthBasketButtonPressed = !table.isMouthBasketButtonPressed;
            }
        },
        toggleMinusButton(tableId) {
            const table = this.gameTables.find(t => t.tableId === tableId);
            if (table) {
                table.isMinusButtonPressed = !table.isMinusButtonPressed;
            }
        },
        DeleteClick(tableId) {
            if (!confirm("Biztosan ki szeretnéd törölni ezt a játékot?")) {
                return;
            }
            axios.delete("https://localhost:7027/api/GameTables/" + tableId)
                 .then((response) => {
                    this.refreshData();
                    alert("Játék sikeresen törölve!");
                 })
                 .catch((error) => {
                    console.error("Error during deleting", error);
                    alert("Hiba történt a törlés során.");
                 });
        },
        hoppKutyaPressed(tableId, playerId){
            const table = this.gameTables.find(t => t.tableId === tableId);
            if (table) {
                let currentTime = new Date()

                let multiplier = 1;
                if(table.isFirstAidButtonPressed){
                    multiplier *= 2;
                }
                if(table.isMouthBasketButtonPressed){
                    multiplier *= 1.5;
                }
                if(table.isMinusButtonPressed){
                    multiplier *= (-1);
                }
                
                let player1CalculatedPoints = table.player1Points;
                let player2CalculatedPoints = table.player2Points;
                if(playerId == 1){
                    player1CalculatedPoints += (1 + (Math.floor((currentTime - new Date(table.lastHoppKutya)) / 1000) /1000000)) * multiplier;
                }
                else{
                    player2CalculatedPoints += (1 + (Math.floor((currentTime - new Date(table.lastHoppKutya)) / 1000) /1000000)) * multiplier;
                }
                axios.put("https://localhost:7027/api/GameTables/" + tableId, {
                        tableId: table.tableId,
                        player1: table.player1,
                        player2: table.player2,
                        player1Points: player1CalculatedPoints,
                        player2Points: player2CalculatedPoints,
                        lastHoppKutya: currentTime.toISOString()},)
                      .then((response) => {
                        this.refreshData(); 
                        alert(response.data);})
                      .catch((error) => {
                        console.error("Error during API call:", error);
                        alert("Hiba történt az API hívás során.");});
            }
        }    
    }
});

app.mount('#app');
