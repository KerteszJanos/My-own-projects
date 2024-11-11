const app = Vue.createApp({
    data() {
        return {
            gameTables: [],
            NewPlayer1Name: '',
            NewPlayer2Name: ''
        }
    },
    mounted() {
        this.refreshData()
    },
    methods: {
        refreshData() {
            axios.get("https://hoppkutya-e3gwgucscagvafbk.italynorth-01.azurewebsites.net/api/GameTables")
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
            axios.delete("https://hoppkutya-e3gwgucscagvafbk.italynorth-01.azurewebsites.net/api/GameTables/" + tableId)
                 .then((response) => {
                    this.refreshData();
                 })
                 .catch((error) => {
                    console.error("Error during deleting", error);
                    alert("Hiba történt a törlés során.");
                 });
        },
        hoppKutyaPressed(tableId, playerId){
            const table = this.gameTables.find(t => t.tableId === tableId);
            if (table) {
                if (!confirm(`Biztosan módosítani szeretnéd a pontszámot a ${table.player1} - ${table.player2} táblán?`)) {
                    return;
                }                

                let currentTime = new Date()
                let lastHoppKutya = new Date(table.lastHoppKutya);

                let multiplier = 1;
                if(lastHoppKutya.getFullYear() != currentTime.getFullYear()) //Yearly
                {
                    multiplier += 10;
                }
                if(lastHoppKutya.getMonth() != currentTime.getMonth()) //Monthly
                {
                    multiplier += 6;
                }
                if(this.getWeekNumber(lastHoppKutya) != this.getWeekNumber(currentTime)) //Weekly
                {
                    multiplier += 3;
                }
                if(lastHoppKutya.getDate() != currentTime.getDate()) //Daily
                {   
                    multiplier += 1.5;
                }
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
                let secondInADay = 60*60*24;

                if(playerId == 1){
                    player1CalculatedPoints += (1 + (Math.floor((currentTime - lastHoppKutya) / 1000) /secondInADay)) * multiplier;
                }
                else{
                    player2CalculatedPoints += (1 + (Math.floor((currentTime - lastHoppKutya) / 1000) /secondInADay)) * multiplier;
                }
                axios.put("https://hoppkutya-e3gwgucscagvafbk.italynorth-01.azurewebsites.net/api/GameTables/" + tableId, {
                        tableId: table.tableId,
                        player1: table.player1,
                        player2: table.player2,
                        player1Points: player1CalculatedPoints,
                        player2Points: player2CalculatedPoints,
                        lastHoppKutya: currentTime.toISOString()},)
                      .then((response) => {
                        this.refreshData();})
                      .catch((error) => {
                        console.error("Error during API call:", error);
                        alert("Hiba történt az API hívás során.");});
            }
        },
        getWeekNumber(date) {
            const d = new Date(Date.UTC(date.getFullYear(), date.getMonth(), date.getDate()));
            
            d.setUTCDate(d.getUTCDate() + 4 - (d.getUTCDay() || 7));
            
            const yearStart = new Date(Date.UTC(d.getUTCFullYear(), 0, 1));
            
            const weekNumber = Math.ceil(((d - yearStart) / 86400000 + 1) / 7);
            
            return weekNumber;
        },
        newGameClick()
        {
            currentTime = new Date();
            axios.post("https://hoppkutya-e3gwgucscagvafbk.italynorth-01.azurewebsites.net/api/GameTables", {
                player1: this.NewPlayer1Name,
                player2: this.NewPlayer2Name,
                player1Points: 0,
                player2Points: 0,
                lastHoppKutya: currentTime.toISOString()},)
              .then((response) => {
                window.location.reload();})
              .catch((error) => {
                console.error("Error during API call:", error);
                alert("Hiba történt az API hívás során.");});
        },
        formatPoints(points) {
            return points.toFixed(2);
        }
    }
});

app.mount('#app');
