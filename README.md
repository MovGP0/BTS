# Bayesian Truth Serum (BTS) Algorithm

Implementation of the BTS algorithm.

## External References

* [Drazen Prelec, A bayesian truth serum for subjective data](http://economics.mit.edu/files/1966)
* [Aeon: Metaknowledge](https://aeon.co/essays/a-mathematical-bs-detector-can-boost-the-wisdom-of-crowds)

## Variables 

````
r:int = index der person die die Antwort gegeben hat 
k:int = index der Antwortmöglichkeit

t[r]:float[] = Antwort der Person (Summe t[r,k] = 1)
t[r,k]:float = einzelne Antwortmöglichkeit der Person

w:T[] = Dinge die wahr sein könnten
p(w):float = Wahrscheinlichkeit dass w wahr ist
p(w|t[r]):float = Wahrscheinlichkeit, dass w wahr ist, wenn Antwort t[r] gegeben wurde
p(t[r,i]|t[s,j]):float ⇒ p(t[,i]|t[,j]):float = Wahrscheinlichkeit, dass die Antwort t[i] gegeben wird, wenn die Antwort t[j] gegeben wurde

x[r,i]:float = Antwort
y[r,i]:float = Vorhersage zur Antwort

ẍ[k] = sum(x[,k])/count(x[,k]) = geom. Mittelwert aller Antworten
log(ÿ[k]) = sum_k (log(y[,k]))/count(y[,k]) = Frequenz einer Vorhersage

I[r] = sum_k(x[r,k]*log(ẍ[k]/ÿ[k]) = Information Score
P[r] sum_k (ẍ[k]*log(y[r,k]/ẍ[k])) = Prediction Score

α:float ∈ 0..1
S[r] = I[r] + α*P[r] = Person score (S=0 ⇒ Vorhersage ist was andere gewählt haben)
````
