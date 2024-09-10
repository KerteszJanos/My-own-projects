import random
import time

def SimulateGame(numberOfTests):
    doors = [0, 0, 1] #kecske, kecske, autó
    playerChoice = 0
    playerWonWithoutSwitch = 0

    for i in range(numberOfTests):
        random.shuffle(doors)
        playerChoice = random.randint(0, 2)
        
        #nincs szükség annak szimulációjára melyik ajtót fedi fel a játékmester
        if doors[playerChoice] == 1:
            playerWonWithoutSwitch += 1

    playerWonPercentage = round(playerWonWithoutSwitch / numberOfTests * 100, 2)
    playerLostPercentage = round((numberOfTests - playerWonWithoutSwitch) / numberOfTests * 100, 2)

    print("\nA játékos " + str(numberOfTests) + " próbálkozásból " + str(playerWonWithoutSwitch) + "-szor nyert csere nélkül és " + str(numberOfTests - playerWonWithoutSwitch) + "-szor cserével.\nEzek %-os aránya:\nCsere nélkül " + str(playerWonPercentage) + "%\nCserével: " + str(playerLostPercentage) + "%\n")

def PrintDoors(doors):
    for door in doors:
        if door == 0:
            print("🚪", end=" ")
        elif door == -1:
            print("🐐", end=" ")
        elif door == 1:
            print("🚗", end=" ")
    

def PlayGame():
    doors = [0, 0, 0] #kecske, kecske, autó
    carLoc = random.randint(0,2)
    print()
    PrintDoors(doors)
    userChoice = int(input("Válassz: 1, 2 vagy 3: "))-1
    
    doorReveals = [0,1,2]
    random.shuffle(doorReveals)
    for reveal in doorReveals:
        if reveal != carLoc and reveal != userChoice:
            doorReveals[0] = reveal
            break
    doors[doorReveals[0]] = -1
    PrintDoors(doors)
    userChoice = int(input("Újra választhatsz: "))-1
    if userChoice == carLoc:
        doors[userChoice] = 1
        PrintDoors(doors)
        print("Nyertél egy autót!\n")
    elif userChoice == doorReveals[0]:
        PrintDoors(doors)
        print("Bizonyára nagyon tetszik az a kecske!\n")
    else:
        doors[userChoice] = -1
        PrintDoors(doors)
        print("Sajnos ez is egy kecske :(\n")
    
def main():
    random.seed(time.time())
    choice = -1;
    while choice != 0:
        choice = int(input("0 - Kilépés\n1 - Szimuláció\n2 - Játék\nVálasztott menüpont: "))
        if choice == 0:
            print("Sziaa!")
        elif choice == 1:
            SimulateGame(1000000)
        elif choice == 2:
            PlayGame()
        else:
            print("A választásod 0, 1 vagy 2 lehet.")


if __name__ == "__main__":
    main()