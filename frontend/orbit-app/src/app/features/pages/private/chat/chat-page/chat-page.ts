import { ChangeDetectionStrategy, Component } from '@angular/core';
import { EnContruccionComponent } from "../../../../../shared/components/en-contruccion-component/en-contruccion-component";

@Component({
  selector: 'app-chat-page',
  imports: [EnContruccionComponent],
  templateUrl: './chat-page.html',
  styleUrl: './chat-page.css',
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class ChatPage {}
