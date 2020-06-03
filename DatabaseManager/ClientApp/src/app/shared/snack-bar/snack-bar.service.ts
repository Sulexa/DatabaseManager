import { Injectable } from '@angular/core';
import { MatSnackBar } from '@angular/material/snack-bar';

@Injectable({
    providedIn: 'root'
})
export class SnackBarService {

    constructor(public snackBar: MatSnackBar) { }

    public success(text: string): void{
        this.snackBar.open(text, null, {
            duration: 3000,
            panelClass: "snack-bar-success"
        });
    }

    public error(text: string): void{
        this.snackBar.open(text, "Fermer", {
            panelClass: "snack-bar-error"
        });
    }
}
