<?php
	// web service, save logs for pointing magnifier. by default, saves to the same directory as this script.
	// parameters:
	// passkey - required, keeps out spam
	// filename - file name. if no file name is specified, use the system time
	// content - content as string
	// image - image file as binary, see the following URL for how to send an image in C#:
	// http://stackoverflow.com/questions/3696401/c-send-image-to-php-script-php-script-loop-through-files-array
	// these can be sent in a GET or POST
	// example GET: https://students.washington.edu/skane/agc/savelog.php?passkey=alex&filename=test&content=what
	//
	// prints "OK" if successful, and an error message otherwise
	
	// app settings
	$passkey = "alex";
	$prependTimestamp = false; // if true, add timestamp (as unix time) to all saved files
	$requireSSL = true; // if true, only work on SSL connections
	
	define ("FILEREPOSITORY","./");
	
	if (($requireSSL == false || $_SERVER['HTTPS'] == "on") && $_REQUEST["passkey"] == $passkey) {
		$id = $_REQUEST["id"];
		$exists = "Found";
		if (! is_dir(FILEREPOSITORY.$id)) {
		   mkdir(FILEREPOSITORY.$id);
		   mkdir(FILEREPOSITORY.$id."/img");
		   $exists = "Created";
		}
		if (! is_dir(FILEREPOSITORY.$id."/img")) {
		   mkdir(FILEREPOSITORY.$id."/img");
		   $exists = "Created";
		}
		if (isset($_REQUEST["initialize"])) { 
			//header("Status:".$exists);
			echo $exists;
        } else if (isset($_REQUEST["content"])) { // text content
			$fname = "";
			$t = time();
			if (isset($_REQUEST["filename"])) {
				if ($prependTimestamp) {
					$fname = $t . "-" . $_REQUEST["filename"] . ".log";
				} else {
					$fname = $t . ".log";
				}
			} else {
				$fname = $t . "-logfile.log";
			}
		
			// save the contents
			file_put_contents(FILEREPOSITORY.$id."/".$fname, $_REQUEST["content"]);
			echo "Content Uploaded";
		} else if (isset($_FILES["image"])) { // uploading an image
			$imname = "";
			$t = time();
			if ($prependTimestamp) {
				$imname = $t . "-" . $_FILES["image"]["name"];
			} else {
				$imname = $_FILES["image"]["name"];
			}
			
			// copy the image file
			move_uploaded_file($_FILES["image"]["tmp_name"], FILEREPOSITORY.$id."/img/".$imname);
			echo "Image Uploaded";
		} else if (isset($_FILES["log"])) { // uploading a log file
			$imname = "";
			$t = time();
			if ($prependTimestamp) {
				$imname = $t . "-" . $_FILES["log"]["name"];
			} else {
				$imname = $_FILES["log"]["name"];
			}
			
			// copy the image file
			move_uploaded_file($_FILES["log"]["tmp_name"], FILEREPOSITORY.$id."/".$imname);
			echo "Log Uploaded";
		}else {
			echo "Error! No log provided.";
		}
		
	} else {
		echo "Error! Invalid parameters.";
	}

?>
