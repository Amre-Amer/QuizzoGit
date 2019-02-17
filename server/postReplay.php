<?php
$servername = "localhost";
$username = "root";
$password = "mysql";
$database = "training";

// Create connection
$conn = new mysqli($servername, $username, $password, $database);

// Check connection
if ($conn->connect_error) {
    die("Connection failed: " . $conn->connect_error);
} 
echo "Connection successful<br>";

$table = "replays";

$firstname = $_POST["firstname"];
$lastname = $_POST["lastname"];
$email = $_POST["email"];
$test = $_POST["test"];
$txtReplays = $_POST["replays"];

//$firstname = "a1";
//$lastname = "a2";
//$email = "b";
//$test = "c";
//$txtReplays = "0,0,0,0,0,0;1,1,1,1,1,1;2,2,2,2,2,2;3,3,3,3,3,3;4,4,4,4,4,4";
//$txtReplays = "0,0,0,0,0,0";

$replays = explode(";", $txtReplays);

$s = "";
$sql = "INSERT INTO $table (firstname, lastname, email, test, posx, posy, posz, eulx, euly, eulz, timestamp) VALUES ";
foreach($replays as $replay) {
    $data = explode(",", $replay);
    $posx = $data[0];
    $posy = $data[1];
    $posz = $data[2];
    $eulx = $data[3];
    $euly = $data[4];
    $eulz = $data[5];
    $sql = $sql.$s."('".$firstname."','".$lastname."','".$email."','".$test."',".$posx.",".$posy.",".$posz.",".$eulx.",".$euly.",".$eulz.",now())";
    $s = ",";
}

echo "<hr>".$sql."<hr>";

if ($conn->query($sql) === TRUE) {
    echo "Post Replay successful<br>";
} else {
    echo "Error: " . $sql . "<br>" . $conn->error;
}

$conn->close();
?>